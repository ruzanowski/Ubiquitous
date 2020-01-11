using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.FetchService.Commands.UpdateProducts;

// ReSharper disable ClassNeverInstantiated.Global

namespace U.FetchService.BackgroundServices
{
    public class ProductsUpdateWorkerHostedService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsUpdateWorkerHostedService> _logger;
        private readonly BackgroundServiceOptions _bgServiceOptions;
        private readonly int _refreshInterval;

        public ProductsUpdateWorkerHostedService(IMediator mediator, ILogger<ProductsUpdateWorkerHostedService> logger,
            BackgroundServiceOptions bgServiceOptions)
        {
            _mediator = mediator;
            _logger = logger;
            _bgServiceOptions = bgServiceOptions;
            _refreshInterval = bgServiceOptions.RefreshSeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            if (_bgServiceOptions.Enabled)
            {
                _logger.LogInformation($"--- Starting gracefully {nameof(ProductsUpdateWorkerHostedService)} ---");
                while (!stopToken.IsCancellationRequested)
                {
                    await SafeUpdate(stopToken);
                    await Task.Delay(TimeSpan.FromSeconds(_refreshInterval), stopToken);
                }

                _logger.LogInformation($"--- Stopping gracefully {nameof(ProductsUpdateWorkerHostedService)} ---");
            }
        }

        private async Task SafeUpdate(CancellationToken stopToken) =>
            await SafeExecution(async () => await _mediator.Send(new FetchProductsCommand(), stopToken));

        /// <summary>
        /// Caution!
        /// Every exception is being caught and ONLY logged
        /// avoiding from shutting down process
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task SafeExecution(Func<Task> action)
        {
            try
            {
                _logger.LogInformation($"--- Executing {nameof(SafeExecution)} ---");
                await action.Invoke();
                _logger.LogInformation($"--- Executing {nameof(SafeExecution)} ---");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}