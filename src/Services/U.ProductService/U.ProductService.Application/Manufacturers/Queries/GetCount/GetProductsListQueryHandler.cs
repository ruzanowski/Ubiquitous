using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Manufacturers.Queries.GetCount
{
    public class GetManufacturersCountQueryHandler : IRequestHandler<GetManufacturersCount, int>
    {
        private readonly ProductContext _context;

        public GetManufacturersCountQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetManufacturersCount request, CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            var count = await products.CountAsync(cancellationToken);

            return count;
        }

        private IQueryable<Manufacturer> GetProductQueryable() => _context.Manufacturers
            .Include(x => x.Pictures)
            .AsQueryable();

    }
}