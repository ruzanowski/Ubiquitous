using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Categories
{
    public class StoreCategoryCommandHandler : IRequestHandler<StoreCategoryCommand, int>
    {
        private readonly SmartStoreContext _context;

        public StoreCategoryCommandHandler(SmartStoreContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(StoreCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                Description = request.Description
            };

            await _context.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}