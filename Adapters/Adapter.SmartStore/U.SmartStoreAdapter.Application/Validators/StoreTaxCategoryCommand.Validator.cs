using FluentValidation;
using U.SmartStoreAdapter.Api.Products;

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