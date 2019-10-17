using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using U.Common.Database;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Infrastructure
{
    public class ProductContextSeeder
    {
        public void SeedAsync(ProductContext context, DbOptions dbOptions, ILogger<ProductContextSeeder> logger)
        {
            if (!dbOptions.Seed)
            {
                logger.LogInformation("Seed has not been activated.");
                return;
            }
            
            var policy = CreatePolicy(logger, nameof(ProductContextSeeder));

            policy.ExecuteAsync(async () =>
            {

                using (context)
                {
                    context.Database.Migrate();

                    if (!context.ProductTypes.Any())
                    {
                        context.ProductTypes.AddRange(GetPredefinedProductTypes());
                    }

                    if (!context.Manufacturers.Any())
                    {
                        context.Manufacturers.AddRange(GetPredefinedManufacturer());
                    }
                    
                    if (!context.Categories.Any())
                    {
                        context.Categories.AddRange(GetPredefinedCategory());
                    }
                    
                    await context.SaveEntitiesAsync();
                }
            }).Wait();
        }

        private IEnumerable<ProductType> GetPredefinedProductTypes()
        {
            return Enumeration.GetAll<ProductType>();
        }

        private Category GetPredefinedCategory()
        {
            return Category.GetDraftCategory();
        }

        private Manufacturer GetPredefinedManufacturer()
        {
            return Manufacturer.GetDraftManufacturer();
        }
        
        private AsyncRetryPolicy CreatePolicy( ILogger<ProductContextSeeder> logger, string prefix, int retries =3)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception,
                            "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                            prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
