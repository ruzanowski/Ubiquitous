using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.IntegrationEvents.Events;
using U.ProductService.Domain.Aggregates.Product;

namespace U.ProductService.Application.Commands
{
    // Regular CommandHandler
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<bool> Handle(CreateProductCommand message, CancellationToken cancellationToken)
        {
            // AddAsync/Update the Buyer AggregateRoot
            // DDD patterns comment: AddAsync child entities and value-objects through the Order Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate
            var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
            var product = new Product(Guid.NewGuid(), address, message.DueDate);

            _logger.LogInformation("--- Creating Product: {@Product} ---", product);

            await _productRepository.AddAsync(product);

            return await _productRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}