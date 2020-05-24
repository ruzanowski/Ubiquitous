using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Category;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;

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

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = await _cachingRepository.GetCachedOrDefaultAsync<Category>($"CategoryAsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var category =  await _context.Categories
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(id));

            if (category)
            {
                await _cachingRepository.CacheAsync(id.ToString(), true);
            }

            return category;
        }

        public async Task<IList<Category>> GetManyAsync()
        {
            var slug = "allCategories";
            var cached = await _cachingRepository.GetCachedOrDefaultAsync<IList<Category>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var categories = await _context.Categories
                .ToListAsync();

            if (categories != null && categories.Any())
            {
                await _cachingRepository.CacheAsync(slug, categories);
            }

            return categories;
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }

        public CategoryRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }
    }
}