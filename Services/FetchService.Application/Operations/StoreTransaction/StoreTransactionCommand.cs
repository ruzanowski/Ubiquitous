using System;
using MediatR;
using U.FetchService.Domain.Entities;

namespace U.FetchService.Application.Operations.StoreTransaction
{
    public class StoreTransactionCommand : IRequest<Guid>
    {
        public BatchTransaction BatchTransaction { get; set; }
    }
}