using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class CategoryRepository : CachingRepository, ICategoryRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;


        public async Task<Category> AddAsync(Category category)
        {
            return (await _context.Categories.AddAsync(category)).Entity;
        }

        public async Task<Category> GetAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            return category;
        }

        public async Task<bool> AnyAsync(Guid id) => await _context.Categories.AnyAsync(x => x.Id.Equals(id));

        public async Task<IList<Category>> GetManyAsync()
        {
            var slug = "allCategories";
            var cached = await GetCachedOrDefaultAsync<IList<Category>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var categories = await _context.Categories
                .ToListAsync();

            if (categories != null && categories.Any())
            {
                await CacheAsync(slug, categories);
            }

            return categories;
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }

        public CategoryRepository(IDistributedCache cache, ProductContext context) : base(cache)
        {
            _context = context;
        }
    }
}