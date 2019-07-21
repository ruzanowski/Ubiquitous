using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using U.Common.Database;

namespace U.ProductService.Persistance.Contexts.Factories
{
    public class ProductContextDesignFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextDesigner.CreateDbContextOptionsBuilder<ProductContext>("../../../../U.ProductService");
            
            return new ProductContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
            {
                await Task.CompletedTask;
            }

            public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                await Task.CompletedTask;
            }

            public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return await Task.FromResult<TResponse>(default(TResponse));
            }

            public async Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                await Task.CompletedTask;
            }
        }
    }
}