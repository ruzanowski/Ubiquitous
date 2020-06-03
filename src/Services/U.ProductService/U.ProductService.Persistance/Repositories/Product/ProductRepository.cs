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
            await _context.AddAsync(product);

            CacheUnderTwoIdentifiers(product);

            return product;
        }

        public void Update(Domain.Entities.Product.Product product)
        {
            _context.Products.Update(product);
            CacheUnderTwoIdentifiers(product);
        }

        public void DeleteProductPictureLink(Domain.Entities.Product.Product product, Guid pictureId)
        {
            var picture = _context.ProductPictures.FirstOrDefault(x => x.PictureId.Equals(pictureId));

            if (picture is null)
                return;

            _context.ProductPictures.Remove(picture);
            CacheUnderTwoIdentifiers(product);
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
            var cached =
                _cachingRepository.Get<Guid?>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var product = await _context.Products
                .Include(x => x.ProductCategory)
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

            return product?.Id;
        }

        public async Task InvalidateCache(Guid productId)
        {
            _cachingRepository.Delete(productId.ToString());
        }

        private void CacheUnderTwoIdentifiers(Domain.Entities.Product.Product product)
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