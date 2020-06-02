using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetCount
{
    public class GetProductsCountQueryHandler : IRequestHandler<GetProductsCount, int>
    {
        private readonly ProductContext _context;
        private const int Days = 14;

        public GetProductsCountQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetProductsCount request, CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            var count = await products
                .Where(x => x.CreatedAt.AddDays(Days) >= DateTime.UtcNow)
                .CountAsync(cancellationToken);

            return count;
        }

        private IQueryable<Product> GetProductQueryable() => _context.Products
            .Include(x => x.Pictures)
            .Include(x => x.ProductCategory)
            .AsQueryable();

    }
}