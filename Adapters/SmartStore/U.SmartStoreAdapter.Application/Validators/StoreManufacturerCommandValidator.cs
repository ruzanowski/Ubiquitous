using FluentValidation;
using U.SmartStoreAdapter.Api.Manufacturers;

namespace U.SmartStoreAdapter.Application.Validators
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