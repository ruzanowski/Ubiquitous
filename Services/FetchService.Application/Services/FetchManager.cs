using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models;
using U.FetchService.Application.Models.AvailableWholesales;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Services
{
    public class FetchManager : IRequest<IEnumerable<SmartProductViewModel>>
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public class Handler : IRequestHandler<FetchManager, IEnumerable<SmartProductViewModel>>
        {
            private readonly IAvailableWholesales _wholesales;
            private readonly ILogger<Handler> _logger;
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            private ConcurrentBag<SmartProductViewModel> _bag;

            public Handler(IAvailableWholesales wholesales, ILogger<Handler> logger)
            {
                _wholesales = wholesales;
                _logger = logger;
                _bag = new ConcurrentBag<SmartProductViewModel>();
            }

            public async Task<IEnumerable<SmartProductViewModel>> Handle(FetchManager request,
                CancellationToken cancellationToken)
            {
                Task.WaitAll(_wholesales.Wholesales.Select(GetAllProductsParallel).ToArray());
                await Task.CompletedTask;
                return _bag;
            }


            private async Task GetAllProductsParallel(IWholesale wholesale)
            {
                
                _logger.LogInformation($"FetchManager: Dispatching wholesale's products fetch - thread {Thread.CurrentThread.ManagedThreadId}");
                var products = await wholesale.FetchProducts();
                if (products is null)
                {
                    _logger.LogInformation($"FetchManager: Fetching failed - thread {Thread.CurrentThread.ManagedThreadId}");
                    return;
                }
                _logger.LogInformation($"FetchManager: Products fetch finished - thread {Thread.CurrentThread.ManagedThreadId}");
                _logger.LogInformation($"FetchManager: Queuing started - thread {Thread.CurrentThread.ManagedThreadId}");
                foreach (var paginatedItem in products.Data ?? new List<SmartProductViewModel>())
                {
                    _bag.Add(paginatedItem);
                }

                _logger.LogInformation($"FetchManager: Queuing finished - thread {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}