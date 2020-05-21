using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SmartStore.Persistance.Context;
using U.Common.Database;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Infrastructure
{
    public class SmartStoreContextSeeder
    {
        public void Seed(SmartStoreContext context, DbOptions dbOptions, ILogger<SmartStoreContextSeeder> logger)
        {
            if (!dbOptions.Seed)
            {
                logger.LogInformation("Seed has not been activated.");
                return;
            }

            var policy = CreatePolicy(logger, nameof(SmartStoreContextSeeder));

            policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();

                    var manufacturers = GetPredefinedManufacturers();
                    if (!context.Manufacturers.Any())
                    {
                        context.Manufacturers.AddRange(manufacturers);
                    }

                    await context.SaveChangesAsync();

                    var categories = GetPredefinedCategories();
                    if (!context.Categories.Any())
                    {
                        context.Categories.AddRange(categories);
                    }

                    await context.SaveChangesAsync();

                    var products = GetPredefinedProduct();
                    if (!context.Products.Any())
                    {
                        context.Products.AddRange(products);
                        await context.SaveChangesAsync();

                        LinkProductsWithManufacturersAndCategories(ref products, categories, manufacturers);

                        await context.SaveChangesAsync();
                    }
                }
            }).Wait();
        }

        private IList<Product> GetPredefinedProduct()
        {
            var fixture = new Fixture();
            var products = new[]
            {
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>(),
                fixture.Create<Product>()
            };

            for (var index = 1; index < products.Length + 1; index++)
            {
                var product = products[index - 1];
                product.Id = index;
            }

            return products;
        }

        private void LinkProductsWithManufacturersAndCategories(ref IList<Product> products,
            IList<Category> categories,
            IList<Manufacturer> manufacturers)
        {
            var random = new Random();
            foreach (var product in products)
            {

                product.Manufacturer = manufacturers[random.Next() % manufacturers.Count];
                product.Category = categories[random.Next() % categories.Count];
            }
        }

        private IList<Category> GetPredefinedCategories()
        {
            var fixture = new Fixture();
            var categories = new[]
            {
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>(),
                fixture.Create<Category>()
            };

            for (var index = 1; index < categories.Length + 1; index++)
            {
                var category = categories[index - 1];
                category.Id = index;
            }

            return categories;
        }

        private IList<Manufacturer> GetPredefinedManufacturers()
        {
            var fixture = new Fixture();
            var manufacturers = new[]
            {
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>(),
                fixture.Create<Manufacturer>()
            };

            for (var index = 1; index < manufacturers.Length + 1; index++)
            {
                var manufacturer = manufacturers[index - 1];
                manufacturer.Id = index;
            }
            return manufacturers;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<SmartStoreContextSeeder> logger, string prefix, int retries = 3)
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