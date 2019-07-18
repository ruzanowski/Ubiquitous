using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using U.EventBus.Abstractions;

namespace U.FetchService.Application.Commands.SendMessage
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SendMessageHandler : IIntegrationEventHandler<Api.SendMessage>
    {

        public async Task Handle(Api.SendMessage @event)
        {
            await Task.CompletedTask; // todo: send message
        }
    }
}