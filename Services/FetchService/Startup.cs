using System;
using System.Net.Http;
using System.Reflection;
using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using U.FetchService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using U.FetchService.Application.Extensions.Hangfire;
using U.FetchService.Application.Operations.Dispatch;
using U.FetchService.Application.Operations.FetchProducts;
using U.FetchService.Persistance.Configuration;
using U.FetchService.Persistance.Context;

namespace U.FetchService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region General & Swagger

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"});
                c.DescribeAllEnumsAsStrings();
            });

            #endregion

            #region services

            services.AddSingleton<HttpClient>();
            services.AddAvailableWholesales(Configuration);
            services.AddSubscribedService(Configuration);

            #endregion

            #region MediatR register

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(FetchDataCommand).GetTypeInfo().Assembly,
            typeof(DispatchCommand).GetTypeInfo().Assembly);

            services.Scan(scan => scan
                .FromAssemblyOf<FetchDataCommand>()
                .AddClasses()
                .AsSelf()
                .WithTransientLifetime());

            #endregion

            #region Logging

            services.AddLogging();

            #endregion

            #region Automapper profiles & init

            var mappingConfig = new MapperConfiguration(mc => { mc.AllowNullCollections = true; });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region DbContext

            services.AddDatabaseContext<FetchServiceContext>(Configuration);

            #endregion
            
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            Configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions.Connection is null)
            {
                throw new Exception("Database options are missing.");
            }
            
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(dbOptions.Connection));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wholesale Manager V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            
            Hangfire.RecurringJob.AddOrUpdate<HangfireMediator>(job => job.SendCommand(new DispatchCommand
                {
                    Executor = "Hangfire",
                    ExecutorComment = "Recurring job"
                }),
                Cron.Minutely,
                TimeZoneInfo.Utc);

        }
    }
}