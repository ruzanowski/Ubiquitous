using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using U.Common.Mvc;
using StackExchange.Redis;

namespace U.Common.Redis
{
    public static class Extensions
    {
        private static string RedisSectionName = "redis";

        public static IServiceCollection AddCustomRedisAndSignalR(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var redisOptions = configuration.GetOptions<RedisOptions>(RedisSectionName);

            services.TryAddSingleton(redisOptions);

            services
                .AddSignalR(options => { options.EnableDetailedErrors = true; })
                .AddJsonProtocol()
                .AddMessagePackProtocol()
                .AddStackExchangeRedis(o =>
                {
                    o.ConnectionFactory = async writer =>
                    {
                        var config = new ConfigurationOptions()
                        {
                            AbortOnConnectFail = false
                        };

                        config.EndPoints.Clear();
                        config.EndPoints.Add($"{redisOptions.Host}", redisOptions.Port);

                        var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                        connection.ConnectionFailed += (_, e) => { Console.WriteLine("Connection to Redis failed."); };

                        if (!connection.IsConnected)
                        {
                            Console.WriteLine("Did not connect to Redis.");
                        }

                        return connection;
                    };
                });

            return services;
        }
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var redisOptions = configuration.GetOptions<RedisOptions>(RedisSectionName);

            services.TryAddSingleton(redisOptions);


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisOptions.Host}:{redisOptions.Port}";
                options.InstanceName = redisOptions.Instance;
            });

            return services;
        }
    }
}