using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Manufacturer;

namespace U.ProductService.Application.Manufacturers.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand, ManufacturerViewModel>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;
        private readonly IMapper _mapper;

        public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository,
            IDomainEventsService domainEventsService,
            IMediator mediator,
            IMapper mapper)
        {
            _manufacturerRepository = manufacturerRepository;
            _domainEventsService = domainEventsService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ManufacturerViewModel> Handle(CreateManufacturerCommand command, CancellationToken cancellationToken)
        {
            var manufacturer = GetManufacturer(command);

            await _manufacturerRepository.AddAsync(manufacturer);
            await _manufacturerRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            var manufacturerViewModel = _mapper.Map<ManufacturerViewModel>(manufacturer);

            return manufacturerViewModel;
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