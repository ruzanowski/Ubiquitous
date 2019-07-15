using System.Collections.Generic;
using MediatR;
using U.FetchService.Domain.Entities;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Commands.ForwardProducts
{
    public class ForwardDataCommand : IRequest<Party>
    {
        public IEnumerable<SmartProductViewModel> Data { get; set; }
    }
}