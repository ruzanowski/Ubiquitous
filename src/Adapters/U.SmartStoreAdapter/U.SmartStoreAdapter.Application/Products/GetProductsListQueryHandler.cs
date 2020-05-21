using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStore.Persistance.Context;
using U.Common.Pagination;
using U.Common.Products;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Products
{
    public class
        GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PaginatedItems<SmartProductViewModel>>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<SmartProductViewModel>> Handle(GetProductsListQuery request,
            CancellationToken cancellationToken)
        {
            var products = _context.Set<Product>()
                .Include(x => x.Category)
                .Include(x => x.Manufacturer)
                .AsQueryable();

            products = new ProductsQueryBuilder(products)
                .FilterByPrice(request.Price)
                .FilterByStockQuantity(request.StockQuantity)
                .Build();

            var productsMapped = _mapper.ProjectTo<SmartProductViewModel>(products);

            var paginatedProducts =
                await PaginatedItems<SmartProductViewModel>.CreateAsync(request.PageIndex, request.PageSize,
                    productsMapped);

            return paginatedProducts;
        }
    }
}