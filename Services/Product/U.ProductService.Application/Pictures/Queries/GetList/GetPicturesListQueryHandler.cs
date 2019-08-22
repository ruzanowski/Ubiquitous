using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Persistance;

namespace U.ProductService.Application.Products.Queries.QueryProducts
{
    public class GetPicturesListQueryHandler : IRequestHandler<GetPicturesListQuery, PaginatedItems<PictureViewModel>>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetPicturesListQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<PictureViewModel>> Handle(GetPicturesListQuery request, CancellationToken cancellationToken)
        {
            var pictures = _context.Pictures.AsQueryable();
            
            var picturesMapped = _mapper.ProjectTo<PictureViewModel>(pictures);

            var paginatedPictures = await PaginatedItems<PictureViewModel>.CreateAsync(request.PageIndex, request.PageSize, picturesMapped);
            
            return paginatedPictures;
        }
    }
}