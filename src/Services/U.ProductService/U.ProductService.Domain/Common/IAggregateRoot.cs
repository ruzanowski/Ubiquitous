
using System;

namespace U.ProductService.Domain.Common
{

    public interface IAggregateRoot
    {
        Guid AggregateId { get; }
        string AggregateTypeName { get; }
    }

}
