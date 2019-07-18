using U.EventBus.Events;

namespace U.FetchService.Api
{
    public class SendMessage : IntegrationEvent
    {
        public string Message { get; }
        public SendMessage(string message)
        {
            Message = message;
        }
    }
}