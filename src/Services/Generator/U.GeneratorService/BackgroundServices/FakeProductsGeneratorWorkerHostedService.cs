using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U.GeneratorService.Services;

// ReSharper disable ClassNeverInstantiated.Global

namespace U.GeneratorService.BackgroundServices
{
    public class FakeProductsGeneratorWorkerHostedService : BackgroundService
    {
        private readonly ILogger<FakeProductsGeneratorWorkerHostedService> _logger;
        private readonly BackgroundServiceOptions _bgServiceOptions;
        private readonly int _refreshInterval;
        private readonly IProductGenerator _generator;
        private readonly ISmartStoreAdapter _smartStoreAdapter;

        public FakeProductsGeneratorWorkerHostedService(ILogger<FakeProductsGeneratorWorkerHostedService> logger,
            BackgroundServiceOptions bgServiceOptions, IProductGenerator generator, ISmartStoreAdapter smartStoreAdapter)
        {
            _logger = logger;
            _bgServiceOptions = bgServiceOptions;
            _generator = generator;
            _smartStoreAdapter = smartStoreAdapter;
            _refreshInterval = bgServiceOptions.RefreshSeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            if (_bgServiceOptions.Enabled)
            {
                _logger.LogInformation($"--- Starting gracefully {nameof(FakeProductsGeneratorWorkerHostedService)} ---");
                while (!stopToken.IsCancellationRequested)
                {
                    await SafeExecution();
                    await Task.Delay(TimeSpan.FromSeconds(_refreshInterval), stopToken);
                }

                _logger.LogInformation($"--- Stopping gracefully {nameof(FakeProductsGeneratorWorkerHostedService)} ---");
            }
        }

        /// Caution!
        /// Every exception is being caught and ONLY logged
        /// avoiding from shutting down process
        private async Task SafeExecution()
        {
            try
            {
                _logger.LogInformation($"--- Executing {nameof(SafeExecution)} ---");
                var fakeProduct = _generator.GenerateFakeProduct();
                await _smartStoreAdapter.StoreProduct(fakeProduct);
                _logger.LogInformation($"--- Executing {nameof(SafeExecution)} ---");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}