using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models;
using U.FetchService.Application.Models.SubscribedServices;
using U.FetchService.Exceptions;

namespace U.FetchService.Extensions
{
    public static class SubscribedServiceExtensions
    {
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
    }
}