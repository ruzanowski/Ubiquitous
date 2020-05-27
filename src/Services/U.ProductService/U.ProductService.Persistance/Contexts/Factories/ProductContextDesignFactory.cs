using Microsoft.EntityFrameworkCore.Design;
using U.Common.NetCore.EF;

namespace U.ProductService.Persistance.Contexts.Factories
{
    public class ProductContextDesignFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<ProductContext>("../../../../");

            return new ProductContext(optionsBuilder.Options);
        }
    }
}