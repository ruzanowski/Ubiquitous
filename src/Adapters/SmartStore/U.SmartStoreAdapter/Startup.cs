using System.Net.Http;
using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using U.SmartStoreAdapter.Application.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartStore.Persistance.Context;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Mvc;
using U.Common.Swagger;
using U.SmartStoreAdapter.Application.Operations.Products;
using U.SmartStoreAdapter.Middleware;

namespace U.SmartStoreAdapter
{
    /// <summary>
    ///
    /// </summary>
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Mini profiler
            services.AddMiniProfiler();

            services.AddCustomMvc()
                .AddSwagger();

            //Services
            services.AddScoped<HttpClient>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(GetProductsListQueryHandler).GetTypeInfo().Assembly);

            //DbContext
            services
                .AddDatabaseContext<SmartStoreContext>(Configuration);

            //Logging Behaviour Pipeline
            services.AddLogging();

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
            }).CreateMapper());

            services.AddConsul();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP transaction pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UsePathBase(Configuration, _logger).Item1
                .UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStore Adapter V1");
                    c.RoutePrefix = string.Empty;
                }).AddExceptionMiddleWare()
                .UseHttpsRedirection()
                .UseMiniProfiler()
                .UseMvc()
                .UseServiceId()
                .UseForwardedHeaders();


            var consulServiceId = app.UseCustomConsul();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }
}