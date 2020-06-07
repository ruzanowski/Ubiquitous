using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using U.Common.NetCore.EF;
using U.Common.NetCore.Http;
using U.ProductService.Application.Infrastructure;
using U.ProductService.Application.Pictures.Commands.Add;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.IntegrationTests.Product.Conventions;
using U.ProductService.Persistance.Contexts;
using Xunit;

namespace U.ProductService.IntegrationTests
{
    public class UtilitiesTestBase : TestBase, IAsyncLifetime
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly ProductContextSeeder Seeder;
        protected readonly IProductRepository ProductRepository;

        protected UtilitiesTestBase()
        {
            Server = CreateServer();
            Client = Server.CreateClient();
            Seeder = new ProductContextSeeder();
            ProductRepository = Server.Host.Services.CreateScope().ServiceProvider.GetService<IProductRepository>();
        }

        private async Task TruncateAndSeedDatabase()
        {
            var context = Server.Host.Services.CreateScope().ServiceProvider.GetService<ProductContext>();

            context.Products.Clear();
            context.ProductTypes.Clear();
            context.Categories.Clear();
            context.Manufacturers.Clear();
            context.Pictures.Clear();
            context.ManufacturerPictures.Clear();
            context.ProductPictures.Clear();

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

        protected async Task<PictureViewModel> AddPicture()
        {
            var addPictureCommand = new AddPictureCommand
            {
                Description = "Picture from Wadowice",
                Url = "http://ubiquitous.com/api/product/picture/2137",
                MimeTypeId = MimeType.Jpg.Id,
                Filename = "Picture #1"
            };
            var addPicturePath = PicturesController.AddPicture();
            var addResponse = await Client.PostAsJsonAsync(addPicturePath, addPictureCommand);
            var addStringResult = await addResponse.Content.ReadAsStringAsync();
            var pictureResult = JsonConvert.DeserializeObject<PictureViewModel>(addStringResult);
            addResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            return pictureResult;
        }

        public async Task InitializeAsync()
        {
            await TruncateAndSeedDatabase();
        }

        public async Task DisposeAsync()
        {
            await TruncateAndSeedDatabase();
        }
    }
}