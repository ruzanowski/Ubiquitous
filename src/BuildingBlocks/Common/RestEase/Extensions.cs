using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;
using U.Common.Consul;
using U.Common.Fabio;
using U.Common.Mvc;

namespace U.Common.RestEase
{
    public static class Extensions
    {
        public static IServiceCollection RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName)
            where T : class
        {
            var clientName = typeof(T).ToString();
            var options = ConfigureOptions(services);
            switch (options.LoadBalancer?.ToLowerInvariant())
            {
                case "consul":
                    ConfigureConsulClient(services, clientName, serviceName);
                    break;
                case "fabio":
                    ConfigureFabioClient(services, clientName, serviceName);
                    break;
                default: 
                    throw new NotImplementedException();
            }

            ConfigureForwarder<T>(services, clientName);

            return services;
        }

        private static RestEaseOptions ConfigureOptions(IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RestEaseOptions>(configuration.GetSection("restEase"));

            return configuration.GetOptions<RestEaseOptions>("restEase");
        }

        private static void ConfigureConsulClient(IServiceCollection services, string clientName,
            string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c =>
                    new ConsulServiceDiscoveryMessageHandler(c.GetService<IConsulServicesRegistry>(),
                        c.GetService<IOptions<ConsulOptions>>(), serviceName, overrideRequestUri: true));
        }

        private static void ConfigureFabioClient(IServiceCollection services, string clientName,
            string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c =>
                    new FabioMessageHandler(c.GetService<IOptions<FabioOptions>>(), serviceName));
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }
    }
}