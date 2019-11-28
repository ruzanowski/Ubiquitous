using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Microsoft.Extensions.Logging;

namespace U.GeneratorService.Services.Generator
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
            var random = new Random();


            var fakeProduct = new SmartProductDto
            {
                Description = GetRandomizedDescription(),
                Height = random.Next(350),
                Width = random.Next(350),
                Length = random.Next(350),
                Weight = random.Next(1300),
                CategoryId = 1,
                Name = FakeNames.GetNames[random.Next(FakeNames.GetNames.Count)],
                InStock = _fixture.Create<int>(),
                ManufacturerId = 10,
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

        private string GetRandomizedDescription()
        {
            var random = new Random();
            var descriptionLong = random.Next() % 5;

            var description = String.Empty;

            for (int i = 0; i < descriptionLong; i++)
            {
                description += FakeNames.GetNames[random.Next(FakeNames.GetNames.Count)] + ". ";
            }

            if (descriptionLong > 3)
            {
                description += FakeNames.GetNames[random.Next(FakeNames.GetNames.Count)] + "!";
            }

            else
            {
                description += FakeNames.GetNames[random.Next(FakeNames.GetNames.Count)] + "?";
            }

            return description;
        }
    }
}