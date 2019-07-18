using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;
using Aggregates = U.ProductService.Domain.Aggregates.Product;

namespace U.ProductService.Persistance.Repositories
{
    public class ProductRepository: Aggregates.IProductRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProductRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Aggregates.Product Add(Aggregates.Product product)
        {
            return  _context.Products.Add(product).Entity;
               
        }

        public async Task<U.ProductService.Domain.Aggregates.Product.Product> GetAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                await _context.Entry(product)
                    .Reference(i => i.Address).LoadAsync();
            }

            return product;
        }

        public void Update(Aggregates.Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
