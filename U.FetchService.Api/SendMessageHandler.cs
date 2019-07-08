using System.Threading;
using System.Threading.Tasks;
using U.Common.Notifications;

namespace U.FetchService.Api
{
    public class SendMessageHandler : IHandler<SendMessage>
    {   
        public async Task HandleAsync(SendMessage @event, CancellationToken token)
        {          
            await Task.CompletedTask;
        }
    }
}