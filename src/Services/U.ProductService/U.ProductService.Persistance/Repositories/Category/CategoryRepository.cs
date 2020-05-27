using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;

        public IUnitOfWork UnitOfWork => _context;


        public async Task<Domain.Aggregates.Category.Category> AddAsync(Domain.Aggregates.Category.Category category)
        {
            return (await _context.Categories.AddAsync(category)).Entity;
        }

        public async Task<Domain.Aggregates.Category.Category> GetAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            return category;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = _cachingRepository.Get<Domain.Aggregates.Category.Category>($"CategoryAsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var category =  await _context.Categories
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(id));

            if (category)
            {
                _cachingRepository.Cache(id.ToString(), true);
            }

            return category;
        }

        public async Task<IList<Domain.Aggregates.Category.Category>> GetManyAsync()
        {
            var slug = "allCategories";
            var cached = _cachingRepository.Get<IList<Domain.Aggregates.Category.Category>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var categories = await _context.Categories
                .ToListAsync();

            if (categories != null && categories.Any())
            {
                _cachingRepository.Cache(slug, categories);
            }

            return categories;
        }

        public void Update(Domain.Aggregates.Category.Category category)
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