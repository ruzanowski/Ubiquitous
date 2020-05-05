using System.Linq;
using U.SmartStoreAdapter.Application.Common.QueryModels;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Products
{
    public class ProductsQueryBuilder
    {
        private IQueryable<Product> _query { get; set; }

        public ProductsQueryBuilder(IQueryable<Product> query)
        {
            _query = query;
        }

        public ProductsQueryBuilder FilterByPrice(PriceWindow priceWindow)
        {
            if (priceWindow != null)
            {
                _query = _query
                    .Where(x => x.Price >= priceWindow.PriceFrom)
                    .Where(x => x.Price <= priceWindow.PriceTo);
            }
            return this;
        }

        public ProductsQueryBuilder FilterByStockQuantity(StockQuantity stockQuantity)
        {
            if (stockQuantity != null)
            {
                _query = _query
                    .Where(x => x.StockQuantity >= stockQuantity.QuantityFrom)
                    .Where(x => x.StockQuantity <= stockQuantity.QuantityTo);
            }
            return this;
        }

        public IQueryable<Product> Build()
        {
            return _query;
        }

    }
}