using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ManufacturerRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Manufacturer> AddAsync(Manufacturer manufacturer)
        {
            return (await _context.Manufacturers.AddAsync(manufacturer)).Entity;
        }

        public async Task<Manufacturer> GetAsync(Guid manufacturerId)
        {
            var product = await _context.Manufacturers
                .Include(x => x.Pictures)
                .FirstOrDefaultAsync(x => x.Id.Equals(manufacturerId));

            return product;
        }

        public async Task<Manufacturer> GetUniqueClientIdAsync(string uniqueClientId)
        {
            var manufacturer =
                await _context.Manufacturers.FirstOrDefaultAsync(x => x.UniqueClientId.Equals(uniqueClientId));
            return manufacturer;
        }

        public async Task<bool> AnyAsync(Guid id) => await _context.Manufacturers.AnyAsync(x => x.Id.Equals(id));

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}