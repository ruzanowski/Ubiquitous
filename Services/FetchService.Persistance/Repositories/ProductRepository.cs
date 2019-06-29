using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.FetchService.Domain.Entities.Product;
using U.FetchService.Persistance.Context;

namespace U.FetchService.Persistance.Repositories
{
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly UmContext _context;
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1,1);

        public ProductRepository(UmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task InsertProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task InsertProductsAsync(IEnumerable<Product> product)
        {
            await Semaphore.WaitAsync();
            try
            {
                await _context.Products.AddRangeAsync(product);
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            Product product = _context.Products.Find(productId);
            _context.Products.Remove(product);
            await Task.CompletedTask;
        }

        public async Task UpdateProductAsync(Product product)
        {
             _context.Entry(product).State = EntityState.Modified;
             await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}