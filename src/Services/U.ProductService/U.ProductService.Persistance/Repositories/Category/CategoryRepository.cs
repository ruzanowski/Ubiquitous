using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.Entities.Product;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;

        public IUnitOfWork UnitOfWork => _context;


        public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
        {
            return (await _context.ProductCategories.AddAsync(productCategory)).Entity;
        }

        public async Task<ProductCategory> GetAsync(Guid categoryId)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);

            return category;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = _cachingRepository.Get<ProductCategory>($"CategoryAsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var category =  await _context.ProductCategories
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(id));

            if (category)
            {
                _cachingRepository.Cache(id.ToString(), true);
            }

            return category;
        }

        public async Task<IList<ProductCategory>> GetManyAsync()
        {
            var slug = "allCategories";
            var cached = _cachingRepository.Get<IList<ProductCategory>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var categories = await _context.ProductCategories
                .ToListAsync();

            if (categories != null && categories.Any())
            {
                _cachingRepository.Cache(slug, categories);
            }

            return categories;
        }

        public void Update(ProductCategory productCategory)
        {
            _context.Entry(productCategory).State = EntityState.Modified;
        }

        public CategoryRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }
    }
}