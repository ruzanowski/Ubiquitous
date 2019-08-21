using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;

namespace U.ProductService.Application.Manufacturers.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, Guid>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        public async Task<Guid> Handle(CreateManufacturerCommand command, CancellationToken cancellationToken)
        {
            var product = GetProduct(command);

            await _manufacturerRepository.AddAsync(product);
            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return product.Id;
        }

        private Manufacturer GetProduct(CreateManufacturerCommand command)
        {
            return new Manufacturer(Guid.NewGuid(),
                command.Name,
                command.Description);
        }
    }
}