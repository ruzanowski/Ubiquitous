using AutoFixture;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.Create.Single;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain.Entities.Manufacturer;

namespace U.ProductService.IntegrationTests.Product.Conventions
{
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


                var manufacturer = Manufacturer.GetDraftManufacturer();

                var dimensions = new DimensionsDto
                {
                    Length = length,
                    Width = width,
                    Height = height,
                    Weight = weight
                };

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
                    manufacturer.Id);

                return command;
            });
        }
    }
}