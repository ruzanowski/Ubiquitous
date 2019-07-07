using System.Collections.Generic;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Operations.FetchProducts
{
    public class FetchDataResult
    {
        public IEnumerable<SmartProductViewModel> Data { get; set; }
        public int ItemsFetched { get; set; }
    }
}