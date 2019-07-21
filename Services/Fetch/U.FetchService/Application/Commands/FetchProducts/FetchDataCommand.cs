using MediatR;
using U.FetchService.Application.Models.Wholesales;

namespace U.FetchService.Application.Commands.FetchProducts
{
    public class FetchDataCommand : IRequest<FetchDataResult>
    {
        public IWholesale Wholesale { get; set; }
    }
}