using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.FetchService.Persistance.Context;

namespace U.FetchService.Application.Operations.StoreTransaction
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class StoreTransactionHandler : IRequestHandler<StoreTransactionCommand, Guid>
    {
        private readonly FetchServiceContext _context;

        public StoreTransactionHandler(FetchServiceContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(StoreTransactionCommand request, CancellationToken cancellationToken)
        {
            await _context.Transactions.AddAsync(request.BatchTransaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return request.BatchTransaction.Id;
        }
    }
}