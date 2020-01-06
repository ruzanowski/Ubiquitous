using System.Linq;
using System.Linq.Dynamic.Core;
using U.SmartStoreAdapter.Application.Models.QueryModels;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Operations.Products
{
    public class ProductsQueryBuilder
    {
        private IQueryable<Product> _query { get; set; }

        public ProductsQueryBuilder(IQueryable<Product> query)
        {
            _query = query;
        }

        public ProductsQueryBuilder FilterByAvailableTime(TimeWindow timeWindow)
        {
            if (timeWindow != null)
            {
                _query = _query
                    .Where(x => x.AvailableStartDateTimeUtc.HasValue)
                    .Where(x => x.AvailableStartDateTimeUtc >= timeWindow.TimeFrom)
                    .Where(x => x.AvailableEndDateTimeUtc.HasValue)
                    .Where(x => x.AvailableEndDateTimeUtc <= timeWindow.TimeTo);
            }
            return this;
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

        public ProductsQueryBuilder FilterByCategory(string category)
        {
            if (category != null)
            {
                _query = _query
                    .Where(x => x.ProductCategories.Any(y => y.Category.Name.Contains(category)));
            }
            return this;
        }

        public ProductsQueryBuilder OrderBy(ProductOrderBy orderBy)
        {
            _query = _query.OrderBy(orderBy.ToString());
            return this;
        }

        public IQueryable<Product> Build()
        {
            return _query;
        }

    }
}