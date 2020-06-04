using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace U.ProductService.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(IDomainEventsService domainEventsService, IMediator mediator, CancellationToken cancellationToken = default);
        void StoreDomainEvents(IDomainEventsService domainEventsService, IMediator mediator, CancellationToken cancellationToken = default);
    }
}
