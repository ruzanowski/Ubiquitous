using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Commands.Update.Single;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;

namespace U.ProductService.Application.Products.Commands.Update
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandlerBase
    {
        protected readonly IProductRepository ProductRepository;
        protected readonly IMediator Mediator;
        protected readonly IDomainEventsService DomainEventsService;

        public UpdateProductCommandHandlerBase(
            IProductRepository productRepository, IMediator mediator, IDomainEventsService domainEventsService)
        {
            ProductRepository = productRepository;
            Mediator = mediator;
            DomainEventsService = domainEventsService;
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await ProductRepository.GetAsync(command.Id, false, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id: '{command.Id}' has not been found");
            }

            var isUpdated = product.UpdateProperties(
                command.Name,
                command.Description,
                command.Price,
                new Dimensions(command.Dimensions.Length,
                    command.Dimensions.Width,
                    command.Dimensions.Height,
                    command.Dimensions.Weight),
                DateTime.UtcNow);

            if (!isUpdated)
            {
                return Unit.Value;
            }

            ProductRepository.Update(product);

            if (!command.QueuedJob?.AutoSave ?? true)
                await ProductRepository.UnitOfWork.SaveEntitiesAsync(DomainEventsService, Mediator,
                    cancellationToken);

            return Unit.Value;
        }
    }
}