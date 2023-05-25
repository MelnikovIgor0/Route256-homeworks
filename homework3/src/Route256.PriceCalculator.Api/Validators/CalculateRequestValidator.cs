using FluentValidation;
using Route256.PriceCalculator.Api.Requests.V2;

namespace Route256.PriceCalculator.Api.Validators;

internal sealed class CalculateRequestValidator: AbstractValidator<CalculateRequest>
{
    public CalculateRequestValidator()
    { 
        RuleForEach(x => x.Goods)
            .SetValidator(new GoodPropertiesValidator());
    }
}