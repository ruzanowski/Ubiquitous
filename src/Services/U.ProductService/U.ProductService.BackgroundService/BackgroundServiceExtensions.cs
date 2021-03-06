﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.Common.NetCore.Mvc;
using U.ProductService.Application.PendingCommands;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Services;

namespace U.ProductService.BackgroundService
{
    public static class BackgroundServiceExtensions
    {
        public static IServiceCollection AddProductBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundServiceOptions = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");

            services.AddSingleton(backgroundServiceOptions);
            var pendingCommands = new PendingCommands();
            services.AddSingleton<IPendingCommands>(pendingCommands);
            services.AddMediatR(typeof(CreateProductCommand).GetTypeInfo().Assembly,
                typeof(CreateProductCommandHandler).GetTypeInfo().Assembly);
            services.AddSingleton<IHostedService>(p =>
                new ProductHostedService(
                    pendingCommands,
                    backgroundServiceOptions,
                    p,
                    p.GetRequiredService<ILogger<ProductHostedService>>()));

            return services;
        }
    }
}