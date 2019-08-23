﻿using System.Linq;
using System.Threading.Tasks;
using MediatR;
using U.ProductService.Domain.SeedWork;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, ProductContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            if (domainEntities.Any())
            {
                var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent));

                await Task.WhenAll(tasks);
            }
        }
    }
}