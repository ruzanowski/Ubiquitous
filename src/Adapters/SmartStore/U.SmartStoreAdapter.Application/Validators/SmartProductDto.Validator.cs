using System.Linq;
using FluentValidation;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Models.Products;

namespace U.SmartStoreAdapter.Application.Validators
{
    public class SmartProductDtoValidator : AbstractValidator<SmartProductDto>
        {
            public SmartProductDtoValidator(SmartStoreContext context, SmartProductDto productDto)
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(1)
                    .MaximumLength(150);
                
                RuleFor(x => x.Description)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(4000);
                
                RuleFor(x => x.Width)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.Height)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                
                RuleFor(x => x.Length)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.Weight)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.InStock)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.IsAvailable)
                    .NotNull();
                
                RuleFor(x => x.ProductCost)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.PriceInTax)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(x => x.ProductUniqueCode)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(1);
                
                RuleFor(x => x.ManufacturerPartNumber)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(1);
                
                RuleFor(x => x.PriceMinimumSpecifiedByCustomer)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);;
                
                RuleFor(x => x.MainPictureId)
                    .Must(id => context.Pictures.Any(product => product.Id.Equals(productDto.MainPictureId)))
                    .When(x=>x.MainPictureId!=null)
                    .WithErrorCode($"Specified MainPictureId: {productDto.MainPictureId} does not exists for table Pictures.")
                    .WithMessage("Please specify existing Picture.");
                
                RuleFor(x => x.ManufacturerId)
                    .NotEmpty()
                    .Must(id => context.Manufacturers.Any(product => product.Id == productDto.ManufacturerId))
                    .WithErrorCode($"Specified ManufacturerId: {productDto.ManufacturerId} does not exists for table Manufacturers.")
                    .WithMessage("Please specify existing manufacturer.");
                
                RuleFor(x => x.CategoryId)
                    .NotNull()
                    .NotEmpty()
                    .Must(id => context.Categories.Any(product => product.Id.Equals(productDto.CategoryId)))
                    .WithErrorCode($"Specified CategoryId: {productDto.CategoryId} does not exists for table Category.")
                    .WithMessage("Please specify existing category.");
                

                foreach (var productDtoPicturesId in productDto.PicturesIds)
                {
                    RuleFor(x => x.PicturesIds)
                        .NotNull()
                        .Must(id => context.Pictures.Any(product => product.Id.Equals(productDtoPicturesId)))
                        .WithErrorCode($"Specified PictureId: {productDtoPicturesId} does not exists for table Picture.")
                        .WithMessage("Please specify existing picture.");
                }
                
            }
        }
}