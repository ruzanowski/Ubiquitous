﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain;
using U.ProductService.Domain.Entities.Manufacturer;

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
            var product = GetManufacturer(command);

            await _manufacturerRepository.AddAsync(product);
            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return product.Id;
        }

        private Manufacturer GetManufacturer(CreateManufacturerCommand command)
        {
            return new Manufacturer(Guid.NewGuid(),
                "not_qualified",
                command.Name,
                command.Description);
        }
    }
}