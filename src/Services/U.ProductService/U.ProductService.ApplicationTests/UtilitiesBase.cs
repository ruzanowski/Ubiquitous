using System;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.NetCore.EF;
using U.Common.Pagination;
using U.ProductService.Application.Infrastructure;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetList;
using U.ProductService.Application.Products.Queries.GetSingle;
using U.ProductService.ApplicationTests.Product;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;
using Xunit;

namespace U.ProductService.ApplicationTests
{
    public class UtilitiesBase : TestBase, IAsyncLifetime
    {
        protected readonly TestServer Server;
        protected readonly IMediator Mediator;
        private readonly ProductContextSeeder Seeder;
        protected readonly IProductRepository ProductRepository;

        protected UtilitiesBase()
        {
            Server = CreateServer();
            Mediator = Server.Host.Services.CreateScope().ServiceProvider.GetService<IMediator>();
            Seeder = new ProductContextSeeder();
            ProductRepository = Server.Host.Services.CreateScope().ServiceProvider.GetService<IProductRepository>();
        }

        protected async Task<PaginatedItems<ProductViewModel>> GetProductsAsync()
        {
            var getProducts = new GetProductsListQuery();

            return await Mediator.Send(getProducts);
        }

        private async Task TruncateAndSeedDatabase()
        {
            var context = Server.Host.Services.CreateScope().ServiceProvider.GetService<ProductContext>();

            context.Products.Clear();
            context.ProductTypes.Clear();
            context.ProductCategories.Clear();
            context.Manufacturers.Clear();
            context.Pictures.Clear();

            await context.SaveChangesAsync();

            var dbOptions = Server.Host.Services.CreateScope().ServiceProvider.GetService<DbOptions>();
            var logger = Server.Host.Services.CreateScope().ServiceProvider.GetService<ILogger<ProductContextSeeder>>();
            var mediator = Server.Host.Services.CreateScope().ServiceProvider.GetService<IMediator>();
            var domainEventsService = Server.Host.Services.CreateScope().ServiceProvider.GetService<IDomainEventsService>();
            await Seeder.SeedAsync(context,
                dbOptions,
                logger,
                mediator,
                domainEventsService);
        }

        protected CreateProductCommand GetCreateProductCommand()
        {
            var fixture = new Fixture().Customize(new CreateProductCustomization());
            return fixture.Create<CreateProductCommand>();
        }

        public async Task InitializeAsync()
        {
            await TruncateAndSeedDatabase();
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }

        protected async Task<ProductViewModel> CreateProductAsync(CreateProductCommand command)
        {
            return await Mediator.Send(command);
        }

        protected async Task<ProductViewModel> GetProductAsync(Guid id) =>
            await Mediator.Send(new QueryProduct(id));
    }
}