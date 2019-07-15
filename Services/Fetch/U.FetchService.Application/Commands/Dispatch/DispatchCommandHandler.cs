using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.FetchService.Application.Commands.FetchProducts;
using U.FetchService.Application.Commands.ForwardProducts;
using U.FetchService.Application.Commands.StoreTransaction;
using U.FetchService.Application.Models.Wholesales;
using U.FetchService.Domain.Entities;

namespace U.FetchService.Application.Commands.Dispatch
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DispatchCommandHandler : IRequestHandler<DispatchCommand>
    {
        private readonly IMediator _mediator;
        private readonly IAvailableWholesales _wholesales;
        public DispatchCommandHandler(IMediator mediator, IAvailableWholesales wholesales)
        {
            _mediator = mediator;
            _wholesales = wholesales;
        }

        public async Task<Unit> Handle(DispatchCommand request, CancellationToken cancellationToken)
        {
            foreach (var wholesale in _wholesales.Wholesales)
            {
                var data = await _mediator.Send(new FetchDataCommand{Wholesale = wholesale}, cancellationToken);
                var destParty = await _mediator.Send(new ForwardDataCommand {Data = data?.Data}, cancellationToken);
                await StoreTransactions(wholesale, data, request, destParty, cancellationToken);   
            }
            return Unit.Value;
        }

        private async Task StoreTransactions(IWholesale wholesale, FetchDataResult data,DispatchCommand request, Party destParty, CancellationToken cancellationToken)
        {
            var executedBy = Executed.Factory.Create(
                request.Executor,
                request.ExecutorComment);
            
            var originParty = Party.Factory.Create(
                wholesale.Settings.Name,
                wholesale.Settings.Ip,
                wholesale.Settings.Port,
                wholesale.Settings.Protocol);

            await _mediator.Send(new StoreTransactionCommand
                {
                    BatchTransaction = BatchTransaction.Factory.Create(executedBy, originParty, destParty, data.ItemsFetched)
                },
                cancellationToken);
        }
    }
}