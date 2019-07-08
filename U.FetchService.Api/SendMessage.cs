using U.Common.Notifications;

namespace U.FetchService.Api
{
    public class SendMessage : IMessage
    {
        public string Message { get; }
        public SendMessage(string message)
        {
            Message = message;
        }
    }
}