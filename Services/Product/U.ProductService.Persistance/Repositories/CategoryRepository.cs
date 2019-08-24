using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Domain;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CategoryRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

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
        public async Task<Category> GetOrCreateDraftCategoryAsync()
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.IsDraft);

            if (category is null)
            {
                var newDraftCategory = (await _context.Categories.AddAsync(Category.GetDraftCategory())).Entity;
                await UnitOfWork.SaveEntitiesAsync();
                return newDraftCategory;
            }

            return category;
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }
    }
}