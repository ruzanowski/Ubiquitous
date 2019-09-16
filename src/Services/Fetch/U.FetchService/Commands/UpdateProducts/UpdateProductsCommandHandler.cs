using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.FetchService.Commands.ForwardProducts;
using U.FetchService.Exceptions;
using U.FetchService.Services;

namespace U.FetchService.Commands.UpdateProducts
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductsCommandHandler : IRequestHandler<UpdateProductsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ISmartStoreAdapter _adapter;

        public UpdateProductsCommandHandler(IMediator mediator, ISmartStoreAdapter adapter)
        {
            _mediator = mediator;
            _adapter = adapter;
        }

        public async Task<Unit> Handle(UpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var data = await _adapter.GetListAsync();

            if (data?.Data is null)
            {
                throw new FetchFailedException();
            }

            if (data.PageSize == 0)
            {
                throw new ZeroProductsFetchedException();
            }

            await _mediator.Send(new ForwardDataCommand(data.Data), cancellationToken);
            return Unit.Value;
        }
    }
}