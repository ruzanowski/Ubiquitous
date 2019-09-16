
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Aggregates.Product
{
	public class ProductType : Enumeration
	{
		public static ProductType SimpleProduct = new ProductType(1, "SimpleProduct");
		public static ProductType GroupedProduct = new ProductType(2, "GroupedProduct");
		public static ProductType BundledProduct = new ProductType(3, "BundledProduct");

		public ProductType(int id, string name)
			: base(id, name)
		{
		}
	}
}
