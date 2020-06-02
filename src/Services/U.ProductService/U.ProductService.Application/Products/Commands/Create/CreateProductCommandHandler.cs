using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Products.Commands.Create
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IManufacturerRepository manufacturerRepository,
            IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
        }

        public async Task<ProductViewModel> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.ExternalProperties?.DuplicationValidated ?? false)
            {
                var duplicate =
                    await _productRepository.GetIdByExternalTupleAsync(
                        command.ExternalProperties.SourceName,
                        command.ExternalProperties.SourceId);

                if (duplicate != null)
                    throw new ProductDuplicatedException(
                        $"Duplicated product with alternative key: '{command.BarCode}'");
            }

            if (command.CategoryId != null)
                await ValidateCategoryOrThrowAsync(command.CategoryId.Value);

            if (command.ManufacturerId != null)
                await ValidateManufacturerOrThrowAsync(command.ManufacturerId.Value);

            var product = GetProduct(command);

            await _productRepository.AddAsync(product);

            if (!command.QueuedJob?.AutoSave ?? true)
                await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return _mapper.Map<ProductViewModel>(product);
        }

        private Product GetProduct(CreateProductCommand command)
        {
            return new Product(command.Name,
                command.Price,
                command.BarCode,
                command.Description,
                new Dimensions(command.Dimensions.Length,
                    command.Dimensions.Width,
                    command.Dimensions.Height,
                    command.Dimensions.Weight),
                command.ManufacturerId ?? Manufacturer.GetDraftManufacturer().Id,
                command.CategoryId ?? ProductCategory.GetDraftCategory().Id,
                ProductType.SimpleProduct.Id,
                command.ExternalProperties?.SourceName,
                command.ExternalProperties?.SourceId);
        }

        private async Task ValidateCategoryOrThrowAsync(Guid categoryId)
        {
            if (!await _categoryRepository.AnyAsync(categoryId))
            {
                throw new ProductCategoryNotFoundException(
                    $"ProductCategory with given primary key: '{categoryId}' has not been found.");
            }
        }

        private async Task ValidateManufacturerOrThrowAsync(Guid id)
        {
            if (!await _manufacturerRepository.AnyAsync(id))
            {
                throw new ManufacturerNotFoundException(
                    $"Manufacturer with given primary key: '{id}' has not been found.");
            }
        }
    }
}