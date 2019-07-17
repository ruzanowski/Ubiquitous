using System.Net.Http;
using System.Reflection;
using AutoMapper;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Installers;
using U.EventBus.Abstractions;
using U.EventBus.RabbitMQ;
using U.IntegrationEventLog;
using U.ProductService.Extensions;
using U.ProductService.Middleware;
using U.ProductService.Persistance;

namespace U.ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<ProductContext>()
                .AddEventBus(Configuration)
                .AddIntegrationEventLogContext(Configuration)
                .AddHealthChecks(Configuration)
                .AddCustomIntegrations(Configuration)
                .AddMediatR(typeof(Startup).GetTypeInfo().Assembly)
                .AddLoggingBehaviour()
                .AddLogging()
                .AddSwaggerGen(c => c.DescribeAllEnumsAsStrings());

            //Services
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                //maps
            }).CreateMapper());

            services.AddScoped<HttpClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseDeveloperExceptionPage();
            app.AddExceptionMiddleWare();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json",
                        "ProductService V1");
                });

            app.UseCors("CorsPolicy");

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();


            //subscribed integration events handlers
        }
    }
}