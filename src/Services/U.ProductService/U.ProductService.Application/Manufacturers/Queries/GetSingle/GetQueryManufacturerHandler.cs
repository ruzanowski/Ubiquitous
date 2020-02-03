using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Manufacturers.Queries.GetSingle
{
    public class GetManufacturerQueryHandler : IRequestHandler<QueryManufacturer, ManufacturerViewModel>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public GetManufacturerQueryHandler(IMapper mapper, IManufacturerRepository manufacturerRepository)
        {
            _mapper = mapper;
            _manufacturerRepository = manufacturerRepository;
        }
        
        public async Task<ManufacturerViewModel> Handle(QueryManufacturer request, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(request.Id);

            if (manufacturer is null)
                throw new ProductNotFoundException($"Manufacturer with primary key: '{request.Id}' has not been found.");
            
            var productsMapped = _mapper.Map<ManufacturerViewModel>(manufacturer);

            return productsMapped;
        }
    }
}