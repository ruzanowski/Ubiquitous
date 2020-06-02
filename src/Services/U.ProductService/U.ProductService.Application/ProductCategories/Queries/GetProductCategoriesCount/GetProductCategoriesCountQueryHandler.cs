using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.ProductCategories.Queries.GetProductCategoriesCount
{
    public class GetCategoriesCountQueryHandler : IRequestHandler<GetProductCategoriesCount, int>
    {
        private readonly ProductContext _context;

        public GetCategoriesCountQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetProductCategoriesCount request, CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            var count = await products.CountAsync(cancellationToken);

            return count;
        }

        private IQueryable<ProductCategory> GetProductQueryable() => _context.ProductCategories
            .AsQueryable();

    }
}