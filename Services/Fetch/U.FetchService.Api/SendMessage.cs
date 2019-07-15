using MediatR;

namespace U.FetchService.Api
{
    public class SendMessage : IRequest
    {
        public string Message { get; }
        public SendMessage(string message)
        {
            Message = message;
        }
    }
}