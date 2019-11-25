﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using U.Common.Consul;
using U.Common.Jwt;
using U.Common.Mvc;
using U.Common.Redis;

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
                .AddLogging()
                .AddConsulServiceDiscovery()
                .AddJwt()
                .AddRedis();

            services.AddOcelot(Configuration)
                .AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseDeveloperExceptionPage();

            app.UseCors
            (b => b
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
            app.UseServiceId();
            app.UseAuthentication();

            app.UseWebSockets();

            app.UseJwtTokenValidator();

            app.UseForwardedHeaders();

            var consulServiceId = app.UseConsulServiceDiscovery();

            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });

            app.UseOcelot().Wait();
        }
    }
}