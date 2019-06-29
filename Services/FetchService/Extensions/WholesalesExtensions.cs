using System.Collections.Generic;
using System.Net.Http;
using U.FetchService.Application.Models;
using U.FetchService.Application.Models.AvailableWholesales;
using U.FetchService.Application.Observables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.SmartStoreAdapter.Api.Endpoints;

namespace U.FetchService.Extensions
{
    public static class WholesalesExtensions
    {
        public static void AddAvailableWholesales(this IServiceCollection services)
        {
            services.AddSingleton<IAvailableWholesales, AvailableWholesales>(provider => new AvailableWholesales
            {
                Wholesales = new List<IWholesale>
                {
                    new SmartWholesale(provider.GetService<HttpClient>(), EndpointsBoard.Port,
                        provider.GetService<ILogger<SmartWholesale>>())
                }
            });
        }
    }
}