using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStore.Persistance.Context;
using U.Common.Pagination;
using U.SmartStoreAdapter.Api.Products;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Operations.Products
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PaginatedItems<SmartProductViewModel>>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<SmartProductViewModel>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = _context.Set<Product>()
                .Include(x => x.ProductPictures)
                .Include(x => x.ProductCategories)
                .AsQueryable();
            
            products = new ProductsQueryBuilder(products)
                .FilterByCategory(request.Category)
                .FilterByPrice(request.Price)
                .FilterByAvailableTime(request.Time)
                .FilterByStockQuantity(request.StockQuantity)
                .OrderBy(request.OrderBy)
                .Build();
            
            var productsMapped = _mapper.ProjectTo<SmartProductViewModel>(products);

            var paginatedProducts = await PaginatedItems<SmartProductViewModel>.PaginatedItemsCreate.CreateAsync(request.PageIndex, request.PageSize, productsMapped);
            
            return paginatedProducts;
        }
    }
}