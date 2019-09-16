using FluentValidation;
using U.SmartStoreAdapter.Application.Models.Products;

namespace U.SmartStoreAdapter.Application.Validators
{
    public class StoreTaxCategoryCommandValidator : AbstractValidator<StoreTaxCategoryCommand>
    {
        public StoreTaxCategoryCommandValidator()
        {
            RuleFor(x => x).SetValidator(new TaxCategoryDtoValidator());
        }
    }
}