using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;

namespace U.ProductService.Persistance.Contexts.Factories
{
    public class ProductContextDesignFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<ProductContext>("../../../../U.ProductService");
            
            return new ProductContext(optionsBuilder.Options);
        }
    }
}