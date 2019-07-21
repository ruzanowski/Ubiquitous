using System;
using MediatR;
using BatchTransaction = U.FetchService.Domain.BatchTransaction;

namespace U.FetchService.Application.Commands.StoreTransaction
{
    public class StoreTransactionCommand : IRequest<Guid>
    {
        public BatchTransaction BatchTransaction { get; set; }
    }
}