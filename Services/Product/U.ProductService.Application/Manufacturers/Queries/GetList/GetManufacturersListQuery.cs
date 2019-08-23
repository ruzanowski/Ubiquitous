using MediatR;
using U.Common.Pagination;
using U.ProductService.Application.Manufacturers.Models;

namespace U.ProductService.Application
{
    public class GetManufacturersListQuery :  IPagination, IRequest<PaginatedItems<ManufacturerViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}