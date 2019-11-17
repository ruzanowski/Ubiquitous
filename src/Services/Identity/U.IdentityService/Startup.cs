﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Authentication;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Mvc;
using U.Common.Redis;
using U.Common.Swagger;
using U.EventBus.RabbitMQ;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Infrastracture;
using U.IdentityService.Persistance.Contexts;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddDatabaseContext<IdentityContext>(Configuration)
                .AddEventBusRabbitMq(Configuration)
                .AddCustomServices()
                .AddLogging()
                .AddSwagger()
                .AddConsul()
//                .AddRedis()
                .AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            var pathBase = app.UsePathBase(Configuration, _logger).Item2;
            app.UseDeveloperExceptionPage()
                .UseCors("CorsPolicy")
                .UseMvcWithDefaultRoute()
                .AddIdentityErrorsHandler()
                .UseSwagger(pathBase)
                .UseServiceId()
                .UseForwardedHeaders()
                .UseAuthentication()
                .UseStaticFiles()
                .UseAccessTokenValidator();

            RegisterConsul(app, applicationLifetime, client);
        }

        private void RegisterConsul(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IConsulClient client)
        {
            var consulServiceId = app.UseCustomConsul();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomServiceRegistrations
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IClaimsProvider, ClaimsProvider>()
                .AddTransient<IRefreshTokenService, RefreshTokenService>()
                .AddTransient<IIdentityService, Application.Services.IdentityService>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

        }
    }
}
