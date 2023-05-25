using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Queries;

public record GetCalculationHistoryQuery(
    long? UserId,
    int Take,
    int Skip,
    long[]? CalculationIds = null
    ) : IRequest<GetHistoryQueryResult>;

public class GetCalculationHistoryQueryHandler 
    : IRequestHandler<GetCalculationHistoryQuery, GetHistoryQueryResult>
{
    private readonly ICalculationService _calculationService;

    public GetCalculationHistoryQueryHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public async Task<GetHistoryQueryResult> Handle(
        GetCalculationHistoryQuery request, 
        CancellationToken cancellationToken)
    {
        var query = new QueryCalculationFilter(
            request.UserId,
            Limit: request.Take,
            Offset: request.Skip,
            request.CalculationIds);

        var log = await _calculationService.QueryCalculations(query, cancellationToken);

        return new GetHistoryQueryResult(
        log.Select(x => new GetHistoryQueryResult.HistoryItem(
                x.TotalVolume, 
                x.TotalWeight,
                x.Price,
                x.GoodIds))
            .ToArray());
    }
}