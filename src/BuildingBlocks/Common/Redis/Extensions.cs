using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using U.Common.Mvc;

namespace U.Common.Redis
{
    public static class Extensions
    {
        private static string RedisSectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            ILogger<StartupBase> logger;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                logger = serviceProvider.GetService<ILogger<StartupBase>>();
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var redisOptions = configuration.GetOptions<RedisOptions>(RedisSectionName);


            services.TryAddSingleton(redisOptions);

            logger.LogInformation("RedisOptions = " + JsonConvert.SerializeObject(redisOptions, Formatting.Indented));

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        new DnsEndPoint(redisOptions?.Host ?? "redis", redisOptions?.Port ?? 6379),
                        new DnsEndPoint("localhost", 6379)
                    },
                    ResolveDns = redisOptions?.ResolveDns ?? true,
                    AbortOnConnectFail = redisOptions?.AbortOnConnectFail ?? false,
                    ServiceName = redisOptions?.ServiceName,
                    ConnectRetry = redisOptions?.ConnectRetry ?? 10,
                    AllowAdmin = redisOptions?.AllowAdmin ?? true
                };
            });

            return services;
        }
    }
}