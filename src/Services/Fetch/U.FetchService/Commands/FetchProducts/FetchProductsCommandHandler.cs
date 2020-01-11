using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.EventBus.Abstractions;
using U.EventBus.Events.Fetch;
using U.FetchService.Exceptions;
using U.FetchService.Services;

namespace U.FetchService.Commands.FetchProducts
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class FetchProductsCommandHandler : IRequestHandler<FetchProductsCommand>
    {
        private readonly ISmartStoreAdapter _adapter;
        private readonly IEventBus _bus;


        public FetchProductsCommandHandler(ISmartStoreAdapter adapter, IEventBus bus)
        {
            _adapter = adapter;
            _bus = bus;
        }

        public async Task<Unit> Handle(FetchProductsCommand request, CancellationToken cancellationToken)
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
                    product.ProductUniqueCode,
                    product.InStock,
                    product.PriceInTax,
                    product.Description,
                    product.Length,
                    product.Width,
                    product.Height,
                    product.Weight,
                    product.MainPictureId,
                    product.CategoryId,
                    product.Id);

                _bus.Publish(@event);
            }

            return Unit.Value;
        }
    }
}