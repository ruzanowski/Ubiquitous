using System;
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
            return  (await _context.Products.AddAsync(product)).Entity;
               
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                await _context.Entry(product).Reference(i => i.Dimensions).LoadAsync();
                await _context.Entry(product).Reference(i => i.Pictures).LoadAsync();
                await _context.Entry(product).Reference(i => i.Pictures).LoadAsync();
            }

            return product;
        }
        
        public async Task<Product> GetByAlternativeIdAsync(string alternateId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.CompareAlternateId(alternateId));
            if (product != null)
            {
                await _context.Entry(product).Reference(i => i.Dimensions).LoadAsync();
                await _context.Entry(product).Reference(i => i.Pictures).LoadAsync();
                await _context.Entry(product).Reference(i => i.Category).LoadAsync();
            }

            return product;
        }

        public async Task<bool> AnyAsync(Guid id) => await _context.Products.AnyAsync(x => x.Id.Equals(id));

        public async Task<bool> AnyAlternateIdAsync(string barCode) =>
            await _context.Products.AnyAsync(x => x.CompareAlternateId(barCode));

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
