using System.Collections.Generic;
using MediatR;
using U.FetchService.Commands.UpdateProducts.ViewModel;

namespace U.FetchService.Commands.ForwardProducts
{
    public class ForwardDataCommand : IRequest
    {
        public ForwardDataCommand(IEnumerable<SmartProductViewModel> data)
        {
            Data = data;
        }
        
        public IEnumerable<SmartProductViewModel> Data { get; private set; }
    }
}