using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Exceptions;
using U.FetchService.Application.Models.Wholesales;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Operations.FetchProducts
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class FetchDataQueryHandler : IRequestHandler<FetchDataCommand, FetchDataResult>
    {
        private readonly ILogger<FetchDataQueryHandler> _logger;
        
        public FetchDataQueryHandler(ILogger<FetchDataQueryHandler> logger)
        {
            _logger = logger;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public async Task<FetchDataResult> Handle(FetchDataCommand request,
            CancellationToken cancellationToken)
        {
            var products = await GetAllProducts(request.Wholesale);

            if (products is null || !products.Any())
                throw new ZeroProductsFetchedException();
            
            return new FetchDataResult
            {
                Data = products,
                ItemsFetched = products.Count()
            };
        }

        private async Task<IEnumerable<SmartProductViewModel>> GetAllProducts(IWholesale wholesale)
        {
            var products = await wholesale.FetchProducts();

            if (!(products?.Data is null))
                return products.Data;
            
            _logger.LogInformation($"FetchManager: Fetching failed - thread {Thread.CurrentThread.ManagedThreadId}");
            throw new FetchFailedException();
        }
    }
}