using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Services
{
    public class FlowManager :  IRequest<IEnumerable<SmartProductViewModel>>
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public class Handler : IRequestHandler<FlowManager, IEnumerable<SmartProductViewModel>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<Handler> _logger;
            
            public Handler(IMediator mediator, ILogger<Handler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<IEnumerable<SmartProductViewModel>> Handle(FlowManager request, CancellationToken cancellationToken)
            {
                var products = await _mediator.Send(new FetchManager(), cancellationToken);

                _logger.LogInformation($"{nameof(FlowManager)} has started a job.");
                await _mediator.Send(new StoringManager {Products = products}, cancellationToken);

                return products;
            }
        }
    }
}