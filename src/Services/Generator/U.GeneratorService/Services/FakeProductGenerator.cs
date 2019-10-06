using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;

namespace U.GeneratorService.Services
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class FakeProductGenerator : IProductGenerator
    {
        private readonly ILogger<FakeProductGenerator> _logger;
        private readonly IFixture _fixture;


        public FakeProductGenerator(ILogger<FakeProductGenerator> logger)
        {
            _logger = logger;
            _fixture = new Fixture();
        }

        public SmartProductDto GenerateFakeProduct()
        {
            var fakeProduct = new SmartProductDto
            {
                Description = _fixture.Create<string>(),
                Height = _fixture.Create<decimal>(),
                Width = _fixture.Create<decimal>(),
                Length = _fixture.Create<decimal>(),
                CategoryId = 1,
                Name = _fixture.Create<string>(),
                Weight = _fixture.Create<decimal>(),
                InStock = _fixture.Create<int>(),
                ManufacturerId = 1,
                ProductCost = _fixture.Create<decimal>(),
                MainPictureId = null,
                ManufacturerPartNumber = _fixture.Create<string>(),
                PriceInTax = _fixture.Create<decimal>(),
                ProductUniqueCode = _fixture.Create<string>(),
                PriceMinimumSpecifiedByCustomer = _fixture.Create<decimal>(),
                IsAvailable = true,
                PicturesIds = new List<int>()
            };

            _logger.LogDebug($"Created new product: {fakeProduct}");
            return fakeProduct;
        }
    }
}