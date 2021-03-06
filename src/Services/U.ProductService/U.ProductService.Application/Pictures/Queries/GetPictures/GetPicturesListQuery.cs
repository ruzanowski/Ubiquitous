using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Pictures.Queries.GetPictures
{
    public class GetPicturesListQuery :  IPagination, IRequest<PaginatedItems<PictureViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}