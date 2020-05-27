using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetStatisticsByCategory
{
    public class GetProductsStatisticsByCategoryQueryHandler : IRequestHandler<GetProductsStatisticsByCategory, IList<ProductByCategoryStatisticsDto>>
    {
        private readonly ProductContext _context;

        public GetProductsStatisticsByCategoryQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductByCategoryStatisticsDto>> Handle(GetProductsStatisticsByCategory request, CancellationToken cancellationToken)
        {
            var notifications = GetQuery();

            var groupedTypes = await notifications
                .GroupBy(g => new
                {
                    g.CategoryId
                })
                .Select(s => new
                {
                    s.Key.CategoryId,
                    Count = s.Count()
                }).ToListAsync(cancellationToken);

            var products = groupedTypes.Select(x => new ProductByCategoryStatisticsDto
            {
                CategoryName = _context.Categories.FirstOrDefault(y => y.Id.Equals(x.CategoryId))?.Name ??
                               "deleted",
                Count = x.Count
            }).ToList();

            return products;
        }


        private IQueryable<Product> GetQuery()
        {
            return _context.Products
                .Include(x => x.Pictures)
                .AsQueryable();
        }
    }
}