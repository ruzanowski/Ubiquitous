using FluentValidation;
using U.SmartStoreAdapter.Application.Manufacturers;

namespace U.SmartStoreAdapter.Application.Common.Validators
{
    public class StoreManufacturerCommandValidator : AbstractValidator<StoreManufacturerCommand>
    {
        public StoreManufacturerCommandValidator()
        {
            RuleFor(x => x.ManufacturerDto)
                .NotNull();
            RuleFor(x => x.ManufacturerDto.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);
            RuleFor(x => x.ManufacturerDto.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}