using System;
using System.Net.Http;
using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartStore.Persistance.Context;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Mvc;
using U.Common.Swagger;
using U.SmartStoreAdapter.Application.Common.MappingProfiles;
using U.SmartStoreAdapter.Application.Infrastructure;
using U.SmartStoreAdapter.Application.Products;
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
            services.AddCustomMvc()
                .AddSwagger();

            //Services
            services.AddScoped<HttpClient>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(GetProductsListQueryHandler).GetTypeInfo().Assembly);

            //DbContext
            services
                .AddDatabaseContext<SmartStoreContext>(Configuration);

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
            }).CreateMapper());

            services.AddConsulServiceDiscovery();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP transaction pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UsePathBase(Configuration, _logger).Item1
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStore Adapter V1");
                    c.RoutePrefix = string.Empty;
                }).AddExceptionMiddleWare()
                .UseServiceId()
                .UseForwardedHeaders()
                .UseMvc();

            Seed(app);
            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }

        private void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                try
                {
                    new SmartStoreContextSeeder()
                        .Seed(serviceScope.ServiceProvider.GetRequiredService<SmartStoreContext>(),
                            serviceScope.ServiceProvider.GetRequiredService<DbOptions>(),
                            serviceScope.ServiceProvider.GetRequiredService<ILogger<SmartStoreContextSeeder>>());
                }
                catch (Exception ex)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}