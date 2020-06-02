using System.Threading;
using System.Threading.Tasks;
using U.Common.Miscellaneous;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.FetchService.Exceptions;

namespace U.FetchService.Services
{
    public interface IProductsDispatcher
    {
        Task FetchAndPublishAsync(CancellationToken cancellationToken);
    }
    public class ProductsDispatcher : IProductsDispatcher
    {
        private readonly ISmartStoreAdapter _adapter;
        private readonly IEventBus _bus;

        public ProductsDispatcher(ISmartStoreAdapter adapter, IEventBus bus)
        {
            _adapter = adapter;
            _bus = bus;
        }

        public async Task FetchAndPublishAsync(CancellationToken cancellationToken)
        {
            var products = await _adapter.GetProductsAsync();

            if (products?.Data is null)
            {
                throw new FetchFailedException();
            }

            if (products.PageSize == 0)
            {
                throw new ZeroProductsFetchedException();
            }

            foreach (var product in products.Data)
            {
                var @event = new NewProductFetchedIntegrationEvent(product.Name,
                    product.ManufacturerId,
                    product.BarCode,
                    product.StockQuantity,
                    product.Price,
                    product.Description,
                    product.Length,
                    product.Width,
                    product.Height,
                    product.Weight,
                    product.CategoryId,
                    product.Id,
                    GlobalConstants.SmartStoreExternalSourceName
                );

                _bus.Publish(@event);
            }
        }
    }
}