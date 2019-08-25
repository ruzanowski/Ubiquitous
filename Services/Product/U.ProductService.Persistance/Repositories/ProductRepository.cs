using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProductRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> AddAsync(Product product)
        {
            return (await _context.Products.AddAsync(product)).Entity;
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            var product = await _context.Products
                .Include(x => x.Dimensions)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            return product;
        }
        
        public async Task<Product> GetByAlternativeIdAsync(string alternateId)
        {
            var product = await _context.Products
                .Include(x => x.Dimensions)
                .Include(x => x.Pictures)
                .Include(x => x.ProductType)
                .FirstOrDefaultAsync(x=>x.BarCode.Equals(alternateId));

            return product;
        }
        
        public async Task<bool> AnyAsync(Guid id) => await _context.Products.AnyAsync(x => x.Id.Equals(id));

        public async Task<bool> AnyAlternateIdAsync(string barCode) =>
            await _context.Products.AnyAsync(x => x.BarCode.Equals(barCode));

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
