using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public ManufacturerRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cachingRepository = cachingRepository;
        }

        public async Task<Manufacturer> AddAsync(Manufacturer manufacturer)
        {
            return (await _context.Manufacturers.AddAsync(manufacturer)).Entity;
        }

        public async Task<Manufacturer> GetAsync(Guid manufacturerId)
        {
            var cached = _cachingRepository.Get<Manufacturer>(manufacturerId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var manufacturer = await _context.Manufacturers
                .Include(x => x.Pictures)
                .FirstOrDefaultAsync(x => x.Id.Equals(manufacturerId));

            if (manufacturer != null)
            {
                _cachingRepository.Cache(manufacturerId.ToString(), manufacturer);
            }

            return manufacturer;
        }

        public async Task<IList<Manufacturer>> GetManyAsync()
        {
            var slug = "allManufacturers";
            var cached = _cachingRepository.Get<List<Manufacturer>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var manufacturers = await _context.Manufacturers
                .ToListAsync();

            if (manufacturers != null && manufacturers.Any())
            {
                _cachingRepository.Cache(slug, manufacturers);
            }

            return manufacturers;
        }

        public async Task<Manufacturer> GetUniqueClientIdAsync(string uniqueClientId)
        {
            var cached = _cachingRepository.Get<Manufacturer>(uniqueClientId);

            if (cached != null)
            {
                return cached;
            }

            var manufacturer =
                await _context
                    .Manufacturers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UniqueClientId.Equals(uniqueClientId));

            if (manufacturer != null)
            {
                _cachingRepository.Cache(uniqueClientId, manufacturer);
            }

            return manufacturer;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = _cachingRepository.Get<Manufacturer>($"Manufacturer_AsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var manufacturer = await _context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

           if (manufacturer != null)
           {
               _cachingRepository.Cache(id.ToString(), manufacturer);
           }

            return manufacturer != null;
        }

        public void Update(Product product)
        {
            _context.SingleUpdateAsync(product);
        }
    }
}