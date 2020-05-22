using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Common.Validators;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Products
{
    public class StoreProductsCommandHandler : IRequestHandler<StoreProductsCommand, int>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreProductsCommandHandler> _logger;

        public StoreProductsCommandHandler(SmartStoreContext context, IMapper mapper, ILogger<StoreProductsCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<int> Handle(StoreProductsCommand command,
            CancellationToken cancellationToken)
        {
            var validator = new SmartProductDtoValidator(_context, command);
            await validator.ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

            var product = _context.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.Category)
                .FirstOrDefault(x =>
                    command.ManufacturerId.Equals(x.ManufacturerId)
                    && x.BarCode == command.BarCode);


            if (product != null)
            {
                _mapper.Map(command, product);
                _context.Update(product);
                await _context.SaveChangesAsync(cancellationToken);
                return product.Id;
            }

            product = _mapper.Map<Product>(command);

            await _context.AddAsync(product, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}