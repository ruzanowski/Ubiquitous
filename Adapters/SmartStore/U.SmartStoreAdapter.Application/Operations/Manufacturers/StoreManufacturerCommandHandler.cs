using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Api.Manufacturers;
using U.SmartStoreAdapter.Application.Validators;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Operations.Manufacturers
{
    public class StoreManufacturerCommandHandler : IRequestHandler<StoreManufacturerCommand, ManufacturerViewModel>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;

        public StoreManufacturerCommandHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ManufacturerViewModel> Handle(StoreManufacturerCommand request, CancellationToken cancellationToken)
        {
            var anyManufacturers = _context.Set<Manufacturer>().Any(x => x.Name == request.ManufacturerDto.Name);
            if (anyManufacturers)
                throw new DuplicateNameException();
            
            var validator = new StoreManufacturerCommandValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
            
            var mappedManufacturer = _mapper.Map<Manufacturer>(request.ManufacturerDto);

            await _context.AddAsync(mappedManufacturer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new ManufacturerViewModel
            {
                Id = mappedManufacturer.Id,
                Name = request.ManufacturerDto.Name,
                Description = request.ManufacturerDto.Description,
                PictureId = request.ManufacturerDto.PictureId
            };
        }
    }
}