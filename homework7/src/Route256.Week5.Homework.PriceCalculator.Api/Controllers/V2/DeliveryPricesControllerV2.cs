using MediatR;
using Microsoft.AspNetCore.Mvc;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V2;
using Route256.Week5.Homework.PriceCalculator.Api.Responses.V1;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;

namespace Route256.Week5.Homework.PriceCalculator.Api.Controllers.V2;

[ApiController]
[Route("/v2/delivery-prices")]
public class DeliveryPricesV2Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryPricesV2Controller(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Метод получения истории вычисления с возможностью фильтрации по UserId или CalculationIds
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public async Task<GetHistoryResponse[]> History(
        GetHistoryV2Request request,
        CancellationToken ct)
    {
        var query = new GetCalculationHistoryQuery(
            request.UserId,
            Take: request.Take,
            Skip: request.Skip,
            request.CalculationIds
        );

        var result = await _mediator.Send(query, ct);

        return result.Items
            .Select(x => new GetHistoryResponse(
                new GetHistoryResponse.CargoResponse(
                    x.Volume,
                    x.Weight,
                    x.GoodIds),
                x.Price))
            .ToArray();
    }
}