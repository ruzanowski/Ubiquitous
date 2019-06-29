using System.Net.Http;
using System.Reflection;
using AutoMapper;
using U.FetchService.Application.Mapping;
using U.FetchService.Application.Services;
using U.FetchService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using U.FetchService.Persistance.Repositories;

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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddAvailableWholesales();
            
            #endregion

            #region MediatR register

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(StoringManager).GetTypeInfo().Assembly,
                typeof(FetchManager).GetTypeInfo().Assembly);

            services.Scan(scan => scan
                .FromAssemblyOf<StoringManager>()
                .FromAssemblyOf<FetchManager>()
                .AddClasses()
                .AsSelf()
                .WithTransientLifetime());

            #endregion

            #region Logging

            services.AddLogging();

            #endregion

            #region Automapper profiles & init

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AllowNullCollections = true;
                ;
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region DbContext

            services.AddUmContext(Configuration);

            #endregion
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
        }
    }
}