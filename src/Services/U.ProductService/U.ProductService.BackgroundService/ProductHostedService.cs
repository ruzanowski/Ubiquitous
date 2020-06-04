using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Services;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;

// ReSharper disable ClassNeverInstantiated.Global

namespace U.ProductService.BackgroundService
{
    [ExcludeFromCodeCoverage]
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
                    var serviceProvider = _provider?.CreateScope().ServiceProvider;
                    var mediator = serviceProvider.GetRequiredService<IMediator>();

                    await SafeExecution(mediator);

                    await Task.Delay(TimeSpan.FromSeconds(_refreshInterval), stopToken);
                }
            }
        }

        private async Task SafeExecution(IMediator mediator)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var createManyCommand = _pendingCommands.GetCreateCommands();
                var updateManyCommand = _pendingCommands.GetUpdateCommands();

                _pendingCommands.Flush();

                await mediator.Send(createManyCommand);
                await mediator.Send(updateManyCommand);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                var createdCount = createManyCommand.CreateProductCommands.Count;
                var updatedCount = updateManyCommand.UpdateProductCommands.Count;
                var total = updatedCount + createdCount;

                _logger.LogInformation(
                    $"Product Background Service has executed {createdCount} creation" +
                    $" and {updatedCount} update jobs (in total {total})" +
                    $" generated in {elapsedMs} ms, {total/(elapsedMs/1000.0):0}/s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}