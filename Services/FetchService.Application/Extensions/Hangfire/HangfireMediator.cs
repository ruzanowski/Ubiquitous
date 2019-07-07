using MediatR;

namespace U.FetchService.Application.Extensions.Hangfire
{
    public class HangfireMediator
    {
        private readonly IMediator _mediator;

        public HangfireMediator(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// workaround
        /// https://stackoverflow.com/questions/43103092/how-to-invoke-async-methods-in-hangfire
        /// </summary>
        /// <param name="request"></param>
        public void SendCommand(IRequest request)
        { 

            _mediator.Send(request).GetAwaiter().GetResult();
        }
    }
}