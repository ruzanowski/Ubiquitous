using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.NetCore.EF;
using U.Common.NetCore.Http;
using U.Common.Pagination;
using U.ProductService.Application.Infrastructure;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.IntegrationTests.Product.Conventions;
using U.ProductService.Persistance.Contexts;
using Xunit;

namespace U.ProductService.IntegrationTests
{

    public class UtilitiesBase : TestBase, IAsyncLifetime
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly ProductContextSeeder Seeder;
        protected readonly IProductRepository ProductRepository;

        protected UtilitiesBase()
        {
            Server = CreateServer();
            Client = Server.CreateClient();
            Seeder = new ProductContextSeeder();
            ProductRepository = Server.Host.Services.CreateScope().ServiceProvider.GetService<IProductRepository>();
        }

        protected async Task<PaginatedItems<ProductViewModel>> GetProductsAsync()
        {
            var httpResponse =  await Client.GetAsync(ProductController.GetProducts());
            return await httpResponse
                .Content
                .ReadAsJsonAsync<PaginatedItems<ProductViewModel>>();
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
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;

            var response = await Client.PostAsJsonAsync(ProductController.CreateProduct(), command);
            response.StatusCode.Should().Be(expectedStatusCode);

            var createResponse = await response.Content.ReadAsJsonAsync<ProductViewModel>();
            return createResponse;
        }

    }
}