using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Pictures.Queries.GetPicture
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
            var picture = await _context.Pictures.FirstOrDefaultAsync(x => x.Id.Equals(request.Id),
                cancellationToken);

           if (picture is null)
            {
                throw new PictureNotFoundException($"Picture with primary key: '{request.Id}' has not been found.");
            }

            var pictureViewModel = _mapper.Map<PictureViewModel>(picture);

            return pictureViewModel;
        }
    }
}