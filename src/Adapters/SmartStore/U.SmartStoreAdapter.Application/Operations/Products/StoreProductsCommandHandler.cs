using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Models.Products;
using U.SmartStoreAdapter.Application.Validators;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.Operations.Products
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
                Product productDb;
                
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        productDb = await StoreOrUpdateProduct(command, cancellationToken);
                    
                        await AddManufacturerAsync(productDb, command);
                        await AddPictures(productDb, command);
                        await AddCategory(productDb, command);
                    
                        await _context.SaveChangesAsync(cancellationToken);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogInformation($"Storing products failed. {ex}");
                        throw new Exception("Storing products failed.");
                    }
                }

                return productDb.Id;
        }

        private async Task<Product> StoreOrUpdateProduct(SmartProductDto product, CancellationToken cancellationToken)
        {
            var sku = $"{product.ManufacturerId}.{product.ProductUniqueCode}";
            var productDb = _context.Products.FirstOrDefault(x => x.Sku == sku);

            var isNull = productDb is null;

            productDb = _mapper.Map(product, isNull ? new Product() : productDb);

            if (!isNull)
            {
                _context.Update(productDb);
                return productDb;
            }

            await _context.AddAsync(productDb, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return productDb;
        }

        private async Task AddManufacturerAsync(Product productDb, SmartProductDto product)
        {
            await _context.AddAsync(new ProductManufacturer
            {
                ProductId = productDb.Id,
                ManufacturerId = product.ManufacturerId
            });
        }

        private async Task AddPictures(Product productDb, SmartProductDto product)
        {
            foreach (var productPicturesId in product.PicturesIds)
            {
                await _context.AddAsync(new ProductPicture
                {
                    ProductId = productDb.Id,
                    PictureId = productPicturesId
                });
            }
        }

        private async Task AddCategory(Product productDb, SmartProductDto product)
        {
            await _context.AddAsync(new ProductCategory
            {
                CategoryId = product.CategoryId,
                ProductId = productDb.Id,
            });
        }
    }
}