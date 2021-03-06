﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using U.Common.NetCore.Auth;
using U.Common.NetCore.Cache;
using U.Common.NetCore.Consul;
using U.Common.NetCore.Jaeger;
using U.Common.NetCore.Mvc;

namespace U.ApiGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services
                .AddConsulServiceDiscovery()
                .AddJwt()
                .AddRedis()
                .AddJaeger();

            services.AddOcelot(Configuration)
                .AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseServiceId()
                .UseAuthentication()
                .UseJwtTokenValidator()
                .UseForwardedHeaders()
                .UseWebSockets()
                .UseRouting()
                .UseCors("CorsPolicy")
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                })
                .UseOcelot()
                .Wait();

            var consulServiceId = app.UseConsulServiceDiscovery();

            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }
}