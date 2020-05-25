using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.SingleInsertAsync(product);
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            return product;
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            var cached = _cachingRepository.Get<Product>(productId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var product = await _context
                .Products
                // .Include(x => x.Category)
                // .Include(x => x.Pictures)
                // .Include(x => x.ProductType)
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            if (product != null)
            {
                _cachingRepository.Cache(productId.ToString(), product);
            }

            return product;
        }

        public async Task<Product> GetByAbsoluteComparerAsync(string externalSourceName, string externalSourceId)
        {
            var cached =
                _cachingRepository.Get<Product>(
                    $"Product_AsNoTracking_{externalSourceName}_{externalSourceId}");

            if (cached != null)
            {
                return cached;
            }

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId.Equals(externalSourceId)
                                          && x.ExternalSourceName.Equals(externalSourceName));

            if (product != null)
            {
                _cachingRepository.Cache(externalSourceId, product);
            }

            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public ProductRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }
    }
}