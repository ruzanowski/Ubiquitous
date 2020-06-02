using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetStatisticsByManufacturers
{
    public class GetProductsStatisticsByManufacturersQueryHandler : IRequestHandler<GetProductsStatisticsByManufacturers, IList<ProductByManufacturersStatisticsDto>>
    {
        private readonly ProductContext _context;

        public GetProductsStatisticsByManufacturersQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductByManufacturersStatisticsDto>> Handle(GetProductsStatisticsByManufacturers request, CancellationToken cancellationToken)
        {
            var notifications = GetQuery();

            var groupedTypes = await notifications
                .GroupBy(g => new
                {
                    g.ManufacturerId
                })
                .Select(s => new
                {
                    s.Key.ManufacturerId,
                    Count = s.Count()
                }).ToListAsync(cancellationToken);

            var products = groupedTypes.Select(x => new ProductByManufacturersStatisticsDto
            {
                ManufacturerName = _context.Manufacturers.FirstOrDefault(y => y.Id.Equals(x.ManufacturerId))?.Name ??
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