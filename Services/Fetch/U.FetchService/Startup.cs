using System.Net.Http;
using System.Reflection;
using AutoMapper;
using U.FetchService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Installers;
using U.EventBus.RabbitMQ;
using U.FetchService.Application.Commands.Dispatch;
using U.FetchService.Application.Commands.FetchProducts;
using U.FetchService.Application.Jobs;
using FetchServiceContext = U.FetchService.Infrastructure.Context.FetchServiceContext;

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

            #region Automapper profiles & init

            var mappingConfig = new MapperConfiguration(mc => { mc.AllowNullCollections = true; });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion


            //DbContext
            services
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<FetchServiceContext>();

            //Hangfire
            services.AddHangFire(Configuration);
                        
            // RabbitMQ Configuration
            services.AddLoggingBehaviour();
            
            //event bus
            services.AddEventBusRabbitMq(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();

            app.UseCustomBackgroundJobs();
        }
    }
}