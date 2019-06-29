using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using U.FetchService.Domain.Entities.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models;
using U.FetchService.Persistance.Repositories;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Services
{
    public class StoringManager : IRequest
    {
        public IEnumerable<SmartProductViewModel> Products { get; set; }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public class Handler : IRequestHandler<StoringManager>
        {
            private readonly IProductRepository _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IMapper mapper, IProductRepository repository, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _repository = repository;
                _logger = logger;
            }

            public async Task<Unit> Handle(StoringManager request, CancellationToken cancellationToken)
            {
                var products = _mapper.Map<IList<Product>>(request.Products);
                _logger.LogInformation($"{nameof(StoringManager)} started a job.");

                foreach (var product in products)
                {
                    var productDb = await _repository.GetProductByIdAsync(product.Id);
                    if (productDb != null)
                    {
                        await _repository.UpdateProductAsync(productDb);
                    }
                    else
                    {
                        await _repository.InsertProductAsync(product);
                    }
                }

                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}