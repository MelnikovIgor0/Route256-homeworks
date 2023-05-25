using FluentValidation;
using Route256.PriceCalculator.Api.Requests.V2;

namespace Route256.PriceCalculator.Api.Validators;

internal sealed class GoodPropertiesValidator: AbstractValidator<GoodProperties>
{
    public GoodPropertiesValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
        
        RuleFor(x => x.Height)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
        
        RuleFor(x => x.Length)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);

        RuleFor(x => x.Width)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
    }
}