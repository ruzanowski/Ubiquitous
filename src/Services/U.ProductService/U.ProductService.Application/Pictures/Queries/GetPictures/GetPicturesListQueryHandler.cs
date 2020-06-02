using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Pictures.Queries.GetPictures
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

            var picturesViewModel = _mapper.ProjectTo<PictureViewModel>(pictures);

            var paginatedPictures2 =
                PaginatedItems<PictureViewModel>.Create(request.PageIndex, request.PageSize, picturesViewModel);

            await Task.CompletedTask;
            return paginatedPictures2;
        }
    }
}