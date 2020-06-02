using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Services;
using U.ProductService.Persistance.Contexts;

// ReSharper disable ClassNeverInstantiated.Global

namespace U.ProductService.BackgroundService
{
    public class ProductHostedService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IPendingCommands _pendingCommands;
        private readonly BackgroundServiceOptions _bgServiceOptions;
        private readonly IServiceProvider _provider;
        private readonly int _refreshInterval;
        private readonly ILogger<ProductHostedService> _logger;

        public ProductHostedService(
            IPendingCommands pendingCommands,
            BackgroundServiceOptions bgServiceOptions,
            IServiceProvider provider,
            ILogger<ProductHostedService> logger)
        {
            _pendingCommands = pendingCommands;
            _bgServiceOptions = bgServiceOptions;
            _provider = provider;
            _logger = logger;
            _refreshInterval = bgServiceOptions.RefreshSeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            if (_bgServiceOptions.Enabled)
            {
                while (!stopToken.IsCancellationRequested)
                {
                    await using var context =
                        _provider?.CreateScope().ServiceProvider.GetRequiredService<ProductContext>();
                    await SafeExecution(context);
                    await Task.Delay(TimeSpan.FromSeconds(_refreshInterval), stopToken);
                }
            }
        }

        private async Task SafeExecution(ProductContext context)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var pendingCreateCommands = _pendingCommands.GetCreateCommands();
                var pendingUpdateCommands = _pendingCommands.GetUpdateCommands();
                _pendingCommands.Flush();

                var mediator = _provider.CreateScope().ServiceProvider
                    .GetRequiredService<IMediator>();

                foreach (var orderedTask in pendingCreateCommands)
                {
                    await mediator.Send(orderedTask);
                }

                foreach (var orderedTask in pendingUpdateCommands)
                {
                    await mediator.Send(orderedTask);
                }

                await context.SaveEntitiesAsync();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                _logger.LogInformation(
                    $"Product Background Service has executed {pendingCreateCommands.Count} creation and {pendingUpdateCommands.Count} update jobs({pendingCreateCommands.Count + pendingUpdateCommands.Count}) in {elapsedMs} ms");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}