using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Categories.Queries.GetCount
{
    public class GetCategoriesCountQueryHandler : IRequestHandler<GetCategoriesCount, int>
    {
        private readonly ProductContext _context;

        public GetCategoriesCountQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetCategoriesCount request, CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            var count = await products.CountAsync(cancellationToken);

            return count;
        }

        private IQueryable<Category> GetProductQueryable() => _context.Categories
            .AsQueryable();

    }
}