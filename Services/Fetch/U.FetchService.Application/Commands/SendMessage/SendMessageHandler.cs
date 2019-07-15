using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.FetchService.Persistance.Messaging;

namespace U.FetchService.Application.Commands.SendMessage
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SendMessageHandler : IRequestHandler<Api.SendMessage>
    {
        private readonly RabbitEventPublisher _bus;

        public SendMessageHandler(RabbitEventPublisher bus)
        {
            _bus = bus;
        }

        public async Task<Unit> Handle(Api.SendMessage request, CancellationToken cancellationToken)
        {
            await _bus.PublishMessage(request);
            
            await Task.CompletedTask;
            return Unit.Value;
        }
    }
}