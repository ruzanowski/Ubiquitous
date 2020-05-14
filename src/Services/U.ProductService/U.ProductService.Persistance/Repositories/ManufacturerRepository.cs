using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ManufacturerRepository : CachingRepository, IManufacturerRepository
    {
        private readonly ProductContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ManufacturerRepository(ProductContext context, IDistributedCache cache) : base(cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Manufacturer> AddAsync(Manufacturer manufacturer)
        {
            return (await _context.Manufacturers.AddAsync(manufacturer)).Entity;
        }

        public async Task<Manufacturer> GetAsync(Guid manufacturerId)
        {
            var cached = await GetCachedOrDefaultAsync<Manufacturer>(manufacturerId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var manufacturer = await _context.Manufacturers
                .Include(x => x.Pictures)
                .FirstOrDefaultAsync(x => x.Id.Equals(manufacturerId));

            if (manufacturer != null)
            {
                await CacheAsync(manufacturerId.ToString(), manufacturer);
            }

            return manufacturer;
        }

        public async Task<IList<Manufacturer>> GetManyAsync()
        {
            var cached = await GetCachedOrDefaultAsync<IList<Manufacturer>>("allManufacturers");

            if (cached != null)
            {
                return cached;
            }

            var manufacturers = await _context.Manufacturers.ToListAsync();

            if (manufacturers != null && manufacturers.Any())
            {
                await CacheAsync("allManufacturers", manufacturers);
            }

            return manufacturers;
        }

        public async Task<Manufacturer> GetUniqueClientIdAsync(string uniqueClientId)
        {
            var cached = await GetCachedOrDefaultAsync<Manufacturer>(uniqueClientId);

            if (cached != null)
            {
                return cached;
            }

            var manufacturer =
                await _context.Manufacturers.FirstOrDefaultAsync(x => x.UniqueClientId.Equals(uniqueClientId));

            if (manufacturer != null)
            {
                await CacheAsync(uniqueClientId, manufacturer);
            }

            return manufacturer;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = await GetCachedOrDefaultAsync<Manufacturer>(id.ToString());

            if (cached != null)
            {
                return true;
            }

            var manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id.Equals(id));

           if (manufacturer != null)
           {
               await CacheAsync(id.ToString(), manufacturer);
           }

            return manufacturer != null;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}