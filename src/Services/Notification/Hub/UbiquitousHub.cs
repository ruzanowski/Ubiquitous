using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DShop.Services.Signalr.Hubs
{
    public class UbiquitousHub : Hub
    {
        private readonly ILogger<UbiquitousHub> _logger;

        public UbiquitousHub(ILogger<UbiquitousHub> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await ConnectAsync();
            }
            catch
            {
                _logger.LogCritical("Failed to connect. Disconnecting.");
                await DisconnectAsync();
            }
        }

        private async Task ConnectAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("connected");
        }

        private async Task DisconnectAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("disconnected");
        }
    }
}