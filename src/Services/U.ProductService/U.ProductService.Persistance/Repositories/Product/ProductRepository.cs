using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Domain.Aggregates.Product.Product> AddAsync(Domain.Aggregates.Product.Product product)
        {
            await _context.AddAsync(product);

            CacheUnderTwoIdentifiers(product);

            return product;
        }

        public void Update(Domain.Aggregates.Product.Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            CacheUnderTwoIdentifiers(product);
        }

        public async Task<Domain.Aggregates.Product.Product> GetAsync(Guid productId)
        {
            var cached = _cachingRepository.Get<Domain.Aggregates.Product.Product>(productId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var product = await _context
                .Products
                .Include(x => x.Category)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .Include(x => x.Dimensions)
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            if (product != null)
            {
                _cachingRepository.Cache(productId.ToString(), product);
            }

            return product;
        }

        public async Task<Guid?> GetAggregateIdByAbsoluteComparerAsync(string externalSourceName, string externalSourceId)
        {
            // _context.ChangeTracker.AutoDetectChangesEnabled = false;

            var cacheKey = $"Product_{externalSourceName}_{externalSourceId}";
            var cached =
                _cachingRepository.Get<Guid?>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .Include(x => x.Dimensions)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ExternalId.Equals(externalSourceId)
                                           && x.ExternalSourceName.Equals(externalSourceName));

            if (product != null)
            {
                CacheUnderTwoIdentifiers(product);
            }

            return product?.AggregateId;
        }

        private void CacheUnderTwoIdentifiers(Domain.Aggregates.Product.Product product)
        {
            _cachingRepository.Cache(product.Id.ToString(), product);

            var cacheKey = $"Product_{product.ExternalSourceName}_{product.ExternalId}";
            _cachingRepository.Cache(cacheKey, product.Id);
        }

        public ProductRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }
    }
}