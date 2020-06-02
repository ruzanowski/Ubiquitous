using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace U.ProductService.Persistance.Contexts
{
    public class NoMediator : IMediator
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.CompletedTask;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            await Task.CompletedTask;
        }
    }
}