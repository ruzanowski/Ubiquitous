using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace U.NotificationService.Hub
{
    public class UbiquitousHub : Microsoft.AspNetCore.SignalR.Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.SendAsync("disconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
        
        
        
    }
}