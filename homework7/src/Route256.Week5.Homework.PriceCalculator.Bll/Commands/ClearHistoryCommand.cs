using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Extensions;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Commands;

public record ClearHistoryCommand(
        long UserId,
        long[] CalculationIds)
    : IRequest<Unit>;

public class ClearHistoryCommandHandler
    : IRequestHandler<ClearHistoryCommand, Unit>
{
    private readonly ICalculationService _calculationService;

    public ClearHistoryCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }


    public async Task<Unit> Handle(
        ClearHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var calculations = await _calculationService.GetCalculation(request.CalculationIds, cancellationToken);

        var goodsIds = calculations
            .SelectMany(_ => _.GoodIds)
            .ToArray();

        if (calculations.Any(_ => _.UserId != request.UserId))
            throw new OneOrManyCalculationsBelongsToAnotherUserException();

        if (calculations.Length != request.CalculationIds.Length)
            throw new OneOrManyCalculationsNotFoundException();

        await _calculationService.DeleteCalculationAndGoods(
            request.CalculationIds,
            goodsIds,
            cancellationToken);

        return Unit.Value;
    }
}