using FluentValidation;
using U.SmartStoreAdapter.Api.Taxes;

namespace U.SmartStoreAdapter.Application.Validators
{
    public class TaxCategoryDtoValidator : AbstractValidator<TaxCategoryDto>
    {
        public TaxCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotNull();
        }
    }
}