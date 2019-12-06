using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;

namespace U.Common.Fabio
{
    public static class Extensions
    {
        public static IServiceCollection AddTypedHttpClient<T>(this IServiceCollection services, string serviceName)
            where T : class
        {
            var clientName = typeof(T).ToString();
            ConfigureFabioClient(services, clientName, serviceName);
            ConfigureForwarder<T>(services, clientName);

            return services;
        }

        private static void ConfigureFabioClient(IServiceCollection services, string clientName,
            string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c => new FabioMessageHandler(c.GetService<IOptions<FabioOptions>>(),
                    services.BuildServiceProvider(),
                    serviceName));
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient(c =>
                new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
                {
                    RequestQueryParamSerializer = new QueryParamSerializer()
                }.For<T>());
        }
    }
}