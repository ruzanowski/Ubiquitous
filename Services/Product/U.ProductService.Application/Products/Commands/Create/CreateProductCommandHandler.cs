using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = GetProduct(command);

            await _productRepository.AddAsync(product);
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return product.Id;
        }

        private Product GetProduct(CreateProductCommand command)
        {
            return new Product(command.Name,
                command.Price,
                command.BarCode,
                command.Description,
                new Dimensions(command.Dimensions.Length,
                    command.Dimensions.Width,
                    command.Dimensions.Height,
                    command.Dimensions.Weight),
                Guid.NewGuid(),
                Guid.NewGuid(),
                ProductType.SimpleProduct);
        }
    }
}