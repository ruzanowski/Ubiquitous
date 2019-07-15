using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Database;
using U.FetchService.Application.Models;
using U.FetchService.Application.Models.SubscribedServices;
using U.FetchService.Application.Models.Wholesales;
using U.FetchService.Exceptions;

namespace U.FetchService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAvailableWholesales(this IServiceCollection services, IConfiguration configuration)
        {
            IEnumerable<PartySettings> availableWholesales = new List<PartySettings>();
            configuration.GetSection("WholesalesSettings").Bind(availableWholesales);

            if (!(availableWholesales is null || availableWholesales.Any()))
                throw new MissingConfigurationException();

            services.AddSingleton<IAvailableWholesales, AvailableWholesales>(
                provider => new AvailableWholesales
                {
                    Wholesales = availableWholesales.Select(wholesale => new SmartWholesale(
                        provider.GetService<HttpClient>(),
                        provider.GetService<ILogger<SmartWholesale>>(),
                        new PartySettings
                        {
                            Ip = wholesale.Ip,
                            Name = wholesale.Name,
                            Port = wholesale.Port,
                            Protocol = wholesale.Protocol
                        }))
                });
        }

        public static void AddSubscribedService(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceSettings = new PartySettings();
            configuration.GetSection("ServiceSettings").Bind(serviceSettings);

            if (serviceSettings.Name is null)
                throw new MissingConfigurationException();

            services.AddSingleton<ISubscribedService, SubscribedService>(
                provider => new SubscribedService
                {
                    Service = new ProductService(
                        provider.GetService<HttpClient>(),
                        provider.GetService<ILogger<ProductService>>(),
                        new PartySettings
                        {
                            Ip = serviceSettings.Ip,
                            Name = serviceSettings.Name,
                            Port = serviceSettings.Port,
                            Protocol = serviceSettings.Protocol
                        })
                });
        }

        public static void AddHangFire(this IServiceCollection services, IConfiguration config)
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            config.GetSection(dbOptionsSection).Bind(dbOptions);

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
        
    }
}