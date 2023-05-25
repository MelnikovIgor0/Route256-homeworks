using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Queries;

public record ClearCalculationHistoryCommand(
    long UserId,
    long[] CalculationIds) : IRequest<ClearHistoryQueryResult>;

public class ClearCalculationHistoryQueryHandler 
    : IRequestHandler<ClearCalculationHistoryCommand, ClearHistoryQueryResult>
{
    private readonly ICalculationService _calculationService;

    public ClearCalculationHistoryQueryHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public async Task<ClearHistoryQueryResult> Handle(
        ClearCalculationHistoryCommand request, 
        CancellationToken cancellationToken)
    {
        var query = new QueryClearHistory(
            request.UserId,
            request.CalculationIds);
        
        await _calculationService.QueryClearHistory(query, cancellationToken);

        return new ClearHistoryQueryResult();
    }
}