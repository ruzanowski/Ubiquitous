using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.Common.Pagination;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PaginatedItems<ProductViewModel>>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<ProductViewModel>> Handle(GetProductsListQuery request,
            CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            if (request.CategoryId != null)
            {
                products = products.Where(x => x.CategoryId.Equals(request.CategoryId));
            }

            if (request.ManufacturerId != null)
            {
                products = products.Where(x => x.ManufacturerId.Equals(request.ManufacturerId));
            }

            var productsMapped = _mapper.ProjectTo<ProductViewModel>(products);

            var paginatedProducts =
                await PaginatedItems<ProductViewModel>.CreateAsync(request.PageIndex, request.PageSize, productsMapped);

            return paginatedProducts;
        }

        private IQueryable<Product> GetProductQueryable() => _context.Products
            .Include(x => x.Pictures)
            .Include(x=>x.Category)
            .AsQueryable();

    }
}