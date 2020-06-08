using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Manufacturer
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public ManufacturerRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }

        public async Task<Domain.Entities.Manufacturer.Manufacturer> AddAsync(Domain.Entities.Manufacturer.Manufacturer manufacturer)
        {
            return (await _context.Manufacturers.AddAsync(manufacturer)).Entity;
        }

        public async Task<Domain.Entities.Manufacturer.Manufacturer> GetAsync(Guid manufacturerId, bool @readonly)
        {
            if (@readonly)
            {
                var cached =
                    _cachingRepository.Get<Domain.Entities.Manufacturer.Manufacturer>(manufacturerId.ToString());

                if (cached != null)
                {
                    return cached;
                }
            }

            var query = _context.Manufacturers
                .Include(x => x.Pictures)
                .ThenInclude(x => x.Picture)
                .ThenInclude(x => x.MimeType)
                .AsQueryable();

            if (@readonly)
                query = query.AsNoTracking();

            var manufacturer =  await query.FirstOrDefaultAsync(x => x.Id.Equals(manufacturerId));

            if (manufacturer != null && @readonly)
            {
                _cachingRepository.Cache(manufacturerId.ToString(), query);
            }

            return manufacturer;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = _cachingRepository.Get<Guid?>($"Manufacturer_AsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var manufacturer = await _context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

           if (manufacturer != null)
           {
               _cachingRepository.Cache(id.ToString(), manufacturer.Id);
           }

            return manufacturer != null;
        }

        public async Task InvalidateCacheAsync(Guid manufacturerId)
        {
            _cachingRepository.Delete(manufacturerId.ToString());
        }

        public void Update(Domain.Entities.Manufacturer.Manufacturer product)
        {
            _context.Update(product);
        }
    }
}