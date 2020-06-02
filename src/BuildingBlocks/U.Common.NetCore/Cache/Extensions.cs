using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using U.Common.NetCore.Mvc;

namespace U.Common.NetCore.Cache
{
    public static class Extensions
    {
        private static string RedisSectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                serviceProvider.GetService<ILogger<StartupBase>>();
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var redisOptions = configuration.GetOptions<RedisOptions>(RedisSectionName);


                if (redisOptions.UseInMemoryAsPrimaryCache)
                {
                    services.AddMemoryCache();
                    services.AddSingleton<ICachingRepository, MemoryCachingRepository>();
                }
                else
                {
                    services.AddSingleton<ICachingRepository, RedisCachingRepository>();
                }

                services.TryAddSingleton(redisOptions);

                if (redisOptions.Enabled)
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.ConfigurationOptions = new ConfigurationOptions
                        {
                            EndPoints =
                            {
                                new DnsEndPoint(redisOptions?.Host ?? "redis", redisOptions?.Port ?? 6379)
                            },
                            ResolveDns = redisOptions?.ResolveDns ?? true,
                            AbortOnConnectFail = redisOptions?.AbortOnConnectFail ?? false,
                            ServiceName = redisOptions?.ServiceName,
                            ConnectRetry = redisOptions?.ConnectRetry ?? 10,
                            AllowAdmin = redisOptions?.AllowAdmin ?? true
                        };
                    });
                }

                return services;
        }
    }

    public interface ICache
    {
        Task<string> GetStringAsync(string cacheKey);
        Task SetStringAsync(string cacheKey, string values, object options);
    }
}