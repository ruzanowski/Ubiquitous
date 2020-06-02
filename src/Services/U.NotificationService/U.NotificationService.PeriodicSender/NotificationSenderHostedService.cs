using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using U.NotificationService.Application.Services.PendingEvents;
using U.NotificationService.Application.SignalR;

// ReSharper disable ClassNeverInstantiated.Global

namespace U.NotificationService.PeriodicSender
{
    public class NotificationSenderHostedService : BackgroundService
    {
        private readonly IPendingEventsService _pendingEventsService;
        private readonly BackgroundServiceOptions _bgServiceOptions;
        private readonly PersistentHub _hub;
        private readonly int _refreshInterval;

        public NotificationSenderHostedService(
            IPendingEventsService pendingEventsService,
            BackgroundServiceOptions bgServiceOptions,
            PersistentHub hub)
        {
            _pendingEventsService = pendingEventsService;
            _bgServiceOptions = bgServiceOptions;
            _hub = hub;
            _refreshInterval = bgServiceOptions.RefreshSeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            if (_bgServiceOptions.Enabled)
            {
                while (!stopToken.IsCancellationRequested)
                {
                    await SafeExecution();
                    await Task.Delay(TimeSpan.FromSeconds(_refreshInterval), stopToken);
                }
            }
        }

        private async Task SafeExecution()
        {
            try
            {
                var pendingEvents = _pendingEventsService.Get();
                await _hub.SaveManyAndSendToAllAsync(pendingEvents);
                _pendingEventsService.Flush();
            }
            catch (Exception)
            {
                //supress
            }
        }
    }
}