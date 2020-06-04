using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Domain.Entities.Product.Product> AddAsync(Domain.Entities.Product.Product product)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            await _context.AddAsync(product);

            _cachingRepository.Cache(product.Id.ToString(), product);

            _context.ChangeTracker.AutoDetectChangesEnabled = true;
            return product;
        }

        public void Update(Domain.Entities.Product.Product product)
        {
            _context.Products.Update(product);
            _cachingRepository.Cache(product.Id.ToString(), product);
        }

        public async Task<Domain.Entities.Product.Product> GetAsync(Guid productId, bool asNoTracking,
            CancellationToken cancellationToken)
        {
            if (asNoTracking)
            {
                var cached = _cachingRepository.Get<Domain.Entities.Product.Product>(productId.ToString());

                if (cached != null)
                {
                    return cached;
                }
            }

            var query = _context
                .Products
                .Include(x => x.ProductCategory)
                .Include(x => x.Pictures)
                .ThenInclude(x => x.Picture)
                .ThenInclude(x => x.MimeType)
                .Include(x => x.ProductType)
                .Include(x => x.Dimensions)
                .AsQueryable();

            if (asNoTracking)
                query = query.AsNoTracking();

            var product =
                await query.FirstOrDefaultAsync(x => x.Id.Equals(productId), cancellationToken: cancellationToken);

            if (product != null && asNoTracking)
            {
                _cachingRepository.Cache(productId.ToString(), query);
            }

            return product;
        }

        public async Task<Guid?> GetIdByExternalTupleAsync(string externalSourceName, string externalSourceId)
        {
            var cacheKey = $"Product_{externalSourceName}_{externalSourceId}";
            var cached = _cachingRepository.Get<Guid?>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var productGuid = _context.Products
                .AsNoTracking()
                .Select(x=> new
                {
                    x.ExternalId,
                    x.ExternalSourceName,
                    x.Id
                })
                .SingleOrDefault(x => x.ExternalId.Equals(externalSourceId)
                                           && x.ExternalSourceName.Equals(externalSourceName))
                ?.Id;

            if (productGuid != null)
            {
                _cachingRepository.Cache(cacheKey, productGuid);
            }

            return productGuid;
        }

        public async Task InvalidateCache(Guid productId)
        {
            _cachingRepository.Delete(productId.ToString());
        }

        public ProductRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }
    }
}