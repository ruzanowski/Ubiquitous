using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain.Common;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Extensions
{
    static class MediatorExtension
    {
        public static IList<INotification> GetDomainEvents(this ProductContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>();

            var entries = domainEntities
                .Where(x => x.Entity.DomainEvents != null
                            && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = entries
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entries.ForEach(entity => entity.Entity.ClearDomainEvents());
            return domainEvents;
        }

        public static async Task DispatchDomainEventsAsync(this IMediator mediator, IDomainEventsService domainEventsService)
        {
            var domainEvents = domainEventsService.GetDomainEvents();

            var tasks = domainEvents
                .Select(async domainEvent => {
                    await mediator.Publish(domainEvent);
                });

            domainEventsService.ClearDomainEvents();

            await Task.WhenAll(tasks);
        }
    }
}