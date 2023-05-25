using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

namespace Route256.Week5.Homework.PriceCalculator.Api.Validators.V1;

public class CalculateRequestValidator : AbstractValidator<CalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.Goods)
            .NotEmpty();

        RuleForEach(x => x.Goods)
            .SetValidator(new CalculateRequestGoodPropertiesValidator());
    }
}