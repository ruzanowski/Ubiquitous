using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ProductRepository: CachingRepository, IProductRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;


        public async Task<Product> AddAsync(Product product)
        {
            return (await _context.Products.AddAsync(product)).Entity;
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            var cached = await GetCachedOrDefaultAsync<Product>(productId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var product = await _context.Products
                .Include(x => x.Dimensions)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            if (product != null)
            {
                await CacheAsync(productId.ToString(), product);
            }

            return product;
        }

        public async Task<Product> GetByAbsoluteComparerAsync(string externalSourceName, string externalSourceId)
        {
            var cached = await GetCachedOrDefaultAsync<Product>(externalSourceName + "_" + externalSourceId);

            if (cached != null)
            {
                return cached;
            }

            var product = await _context.Products
                .Include(x => x.Dimensions)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.EqualsAbsoluteExternalKey(externalSourceName, externalSourceId));

            if (product != null)
            {
                await CacheAsync(externalSourceId, product);
            }

            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public ProductRepository(IDistributedCache cache, ProductContext context) : base(cache)
        {
            _context = context;
        }
    }
}
