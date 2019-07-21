using System.Collections.Generic;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Commands.FetchProducts
{
    public class FetchDataResult
    {
        public IEnumerable<SmartProductViewModel> Data { get; set; }
        public int ItemsFetched { get; set; }
    }
}