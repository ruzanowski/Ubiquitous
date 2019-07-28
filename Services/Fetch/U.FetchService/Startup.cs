using System.Net.Http;
using System.Reflection;
using AutoMapper;
using Consul;
using Hangfire;
using U.FetchService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Fabio;
using U.Common.Mvc;
using U.Common.Pipeline;
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
            services
                .AddCustomMvc()
                .AddAvailableWholesales(Configuration)
                .AddSubscribedService(Configuration)
                .AddCustomMediatR()
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<FetchServiceContext>()
                .AddLoggingBehaviour()
                .AddHangFire(Configuration)
                .AddEventBusRabbitMq(Configuration)
                .AddCustomConsul()
                .AddCustomFabio();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] {new HangfireAuthorizationFilter()}
            });
            app.UseCustomBackgroundJobs();
            
            var consulServiceId = app.UseCustomConsul();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(FetchDataCommand).GetTypeInfo().Assembly,
                typeof(DispatchCommand).GetTypeInfo().Assembly);
            return services;
        }
    }
}