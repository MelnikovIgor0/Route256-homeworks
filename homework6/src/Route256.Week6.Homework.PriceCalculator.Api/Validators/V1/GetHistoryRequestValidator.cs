using FluentValidation;
using Route256.Week5.Workshop.PriceCalculator.Api.Requests.V1;

namespace Route256.Week5.Workshop.PriceCalculator.Api.Validators.V1;

public class GetHistoryRequestValidator : AbstractValidator<GetHistoryRequest>
{
    public GetHistoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
        RuleFor(x => x.Take)
            .GreaterThan(0);
    }
}