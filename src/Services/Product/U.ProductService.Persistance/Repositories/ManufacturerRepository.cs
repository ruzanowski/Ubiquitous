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

        public async Task<Manufacturer> AddAsync(Manufacturer product)
        {
            return (await _context.Manufacturers.AddAsync(product)).Entity;
        }

        public async Task<Manufacturer> GetAsync(Guid productId)
        {
            var product = await _context.Manufacturers.FindAsync(productId);
            if (product != null)
            {
                await _context.Entry(product).Reference(i => i.Pictures).LoadAsync();
            }

            return product;
        }

        public async Task<Manufacturer> GetUniqueClientIdAsync(string uniqueClientId)
        {
            var product =
                await _context.Manufacturers.FirstOrDefaultAsync(x => x.UniqueClientId.Equals(uniqueClientId));
            return product;
        }

        public async Task<bool> AnyAsync(Guid id) => await _context.Manufacturers.AnyAsync(x => x.Id.Equals(id));

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}