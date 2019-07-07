using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using U.FetchService.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models.Wholesales;
using U.FetchService.Exceptions;

namespace U.FetchService.Extensions
{
    public static class WholesalesExtensions
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
                    Wholesales = provider.GetWholesales(availableWholesales)
                });
        }

        private static IEnumerable<IWholesale> GetWholesales(this IServiceProvider provider,
            IEnumerable<PartySettings> wholesalesSettings)
        {
            return wholesalesSettings.Select(wholesale => new SmartWholesale(
                provider.GetService<HttpClient>(),
                provider.GetService<ILogger<SmartWholesale>>(),
                new PartySettings
                {
                    Ip = wholesale.Ip,
                    Name = wholesale.Name,
                    Port = wholesale.Port,
                    Protocol = wholesale.Protocol
                }));
        }
    }
}