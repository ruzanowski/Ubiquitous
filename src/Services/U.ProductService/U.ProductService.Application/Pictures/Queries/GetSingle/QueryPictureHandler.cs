using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Pictures.Queries.GetSingle
{
    public class GetPictureQueryHandler : IRequestHandler<GetPictureQuery, PictureViewModel>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetPictureQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PictureViewModel> Handle(GetPictureQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Pictures.FindAsync(request.Id);

            if (products is null)
                throw new PictureNotFoundException($"Picture with primary key: '{request.Id}' has not been found.");
            
            var productsMapped = _mapper.Map<PictureViewModel>(products);

            return productsMapped;
        }
    }
}