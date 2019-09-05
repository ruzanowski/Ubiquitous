
using System;

namespace U.ProductService.Domain.SeedWork
{

    public interface IAggregateRoot
    {
        Guid AggregateId { get; }
        string AggregateTypeName { get; }
    }

}
