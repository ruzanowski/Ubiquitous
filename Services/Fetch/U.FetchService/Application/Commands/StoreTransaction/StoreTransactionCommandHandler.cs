using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FetchServiceContext = U.FetchService.Infrastructure.Context.FetchServiceContext;

namespace U.FetchService.Application.Commands.StoreTransaction
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