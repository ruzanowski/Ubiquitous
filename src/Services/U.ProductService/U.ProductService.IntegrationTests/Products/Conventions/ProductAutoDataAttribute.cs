using System;
using AutoFixture;
using AutoFixture.Xunit2;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.IntegrationTests.Products.Conventions
{
    public class ProductAutoDataAttribute : AutoDataAttribute
    {
        public ProductAutoDataAttribute() : base(() => new Fixture().Customize(new CreateProductCustomization()))
        {
        }
    }


    public class CreateProductCustomization : CompositeCustomization, ICustomization
    {
        public new void Customize(IFixture fixture)
        {
            fixture.Register<CreateProductCommand>(() =>
            {
                var name = fixture.Create<string>();
                var barCode = fixture.Create<string>();
                var price = fixture.Create<decimal>();
                var description = fixture.Create<string>();
                var externalSourceName = fixture.Create<string>();
                var externalSourceId = fixture.Create<string>();
                var length = fixture.Create<decimal>();
                var weight = fixture.Create<decimal>();
                var height = fixture.Create<decimal>();
                var width = fixture.Create<decimal>();


                var manufacturer = fixture.Create<Guid>();

                var dimensions = new DimensionsDto
                {
                    Length = length,
                    Width = width,
                    Height = height,
                    Weight = weight
                };

                //todo: assure that manufacturerId really exists,because now it's wrong guid.
                var command = new CreateProductCommand(name,
                    barCode,
                    price,
                    description,
                    dimensions,
                    new ExternalCreation
                    {
                        DuplicationValidated = true,
                        SourceId = externalSourceName,
                        SourceName = externalSourceId
                    },
                    manufacturer);

                return command;
            });
        }
    }
}