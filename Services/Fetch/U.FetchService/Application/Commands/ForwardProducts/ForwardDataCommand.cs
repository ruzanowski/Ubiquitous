using System.Collections.Generic;
using MediatR;
using U.SmartStoreAdapter.Api.Products;
using Party = U.FetchService.Domain.Party;

namespace U.FetchService.Application.Commands.ForwardProducts
{
    public class ForwardDataCommand : IRequest<Party>
    {
        public IEnumerable<SmartProductViewModel> Data { get; set; }
    }
}