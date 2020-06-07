using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using U.Common.NetCore.EF;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Infrastructure
{
    public class ProductContextSeeder
    {
        public async Task SeedAsync(ProductContext context, DbOptions dbOptions, ILogger<ProductContextSeeder> logger, IMediator mediator, IDomainEventsService domainEventsService)
        {
            if (!dbOptions.Seed)
            {
                logger.LogInformation("Seed has not been activated.");
                return;
            }

            var policy = CreatePolicy(logger, nameof(ProductContextSeeder));

            await policy.ExecuteAsync(async () =>
            {
                await using (context)
                {
                    await context.Database.MigrateAsync();

                    if (!context.ProductTypes.Any())
                    {
                        await context.ProductTypes.AddRangeAsync(GetPredefinedProductTypes());
                    }

                    if (!context.MimeTypes.Any())
                    {
                        await context.MimeTypes.AddRangeAsync(GetPredefinedMimeTypes());
                    }

                    if (!context.Manufacturers.Any())
                    {
                        await context.Manufacturers.AddRangeAsync(GetPredefinedManufacturer());
                    }

                    if (!context.Categories.Any())
                    {
                        await context.Categories.AddRangeAsync(GetPredefinedCategory());
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<ProductType> GetPredefinedProductTypes()
        {
            return Enumeration.GetAll<ProductType>();
        }

        private IEnumerable<MimeType> GetPredefinedMimeTypes()
        {
            return Enumeration.GetAll<MimeType>();
        }

        private Category GetPredefinedCategory()
        {
            return Category.GetDraftCategory();
        }

        private IEnumerable<Manufacturer> GetPredefinedManufacturer()
        {
            yield return Manufacturer.GetDraftManufacturer();

            yield return new Manufacturer(Guid.Parse("0ed65687-35c3-4ca2-82bd-8e01ea80ae86"),
                "Ubi-Man",
                "Ubi-Man",
                "Ubiquitous shop, selling everything!");

            yield return new Manufacturer(Guid.Parse("990ac537-b7f7-46e4-bc8f-2a743a6cdaeb"),
                "Shopcom",
                "Shopcom",
                "Shop that sells everything online!");

            yield return new Manufacturer(Guid.Parse("7b671d69-990a-490d-80bb-2d88a8b3f797"),
                "Fake-shop",
                "Fake-shop",
                "Does not sell anything, it's a fake shop!");

            yield return new Manufacturer(Guid.Parse("d28d5a73-9ebe-4cc9-b473-b5a50c6d038c"),
                "GigaWholesaleR",
                "GigaWholesaleR",
                "Wholesale, shipping everywhere, selling worldwide!");

        }

        private AsyncRetryPolicy CreatePolicy(ILogger<ProductContextSeeder> logger, string prefix, int retries = 3)
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