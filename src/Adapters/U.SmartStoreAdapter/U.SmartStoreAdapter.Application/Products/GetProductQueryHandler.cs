using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Common.Exceptions;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, SmartProductViewModel>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SmartProductViewModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Set<Product>()
                .Include(x => x.ProductCategories)
                .FirstOrDefaultAsync(x => x.Sku == request.Sku || x.Id == request.Id,
                    cancellationToken: cancellationToken);

            if (products is null)
                throw new ObjectNotFoundException();

            var productsMapped = _mapper.Map<SmartProductViewModel>(products);

            return productsMapped;
        }
    }
}