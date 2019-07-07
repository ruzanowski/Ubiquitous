using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using U.FetchService.Application.Models.SubscribedServices;
using U.FetchService.Domain.Entities;

namespace U.FetchService.Application.Operations.ForwardProducts
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ForwardDataCommandHandler : IRequestHandler<ForwardDataCommand, Party>
    {
        private readonly IService _service;
        //todo: forward data to subscribed entities(product service) with RabbitMq
        public ForwardDataCommandHandler(ISubscribedService subscribedService)
        {
            _service = subscribedService.Service;
        }
        
        public async Task<Party> Handle(ForwardDataCommand request, CancellationToken cancellationToken)
        {
            await _service.ForwardData(request.Data);

            return Party.Factory.Create(
                _service.Settings.Name,
                _service.Settings.Ip,
                _service.Settings.Port,
                _service.Settings.Protocol);
        }
    }
}