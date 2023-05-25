using MediatR;
using Route256.Week5.Workshop.PriceCalculator.Bll.Extensions;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Workshop.PriceCalculator.Bll.Commands;

public record CalculateDeliveryPriceCommand(
        long UserId,
        GoodModel[] Goods)
    : IRequest<CalculateDeliveryPriceResult>;

public class CalculateDeliveryPriceCommandHandler 
    : IRequestHandler<CalculateDeliveryPriceCommand, CalculateDeliveryPriceResult>
{
    private readonly ICalculationService _calculationService;

    public CalculateDeliveryPriceCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public async Task<CalculateDeliveryPriceResult> Handle(
        CalculateDeliveryPriceCommand request, 
        CancellationToken cancellationToken)
    {
        request.EnsureHasGoods();

        var volumePrice = _calculationService.CalculatePriceByVolume(request.Goods, out var volume);
        var weightPrice = _calculationService.CalculatePriceByWeight(request.Goods, out var weight);
        var resultPrice = Math.Max(volumePrice, weightPrice);

        var model = new SaveCalculationModel(
            request.UserId, 
            volume, 
            weight,
            resultPrice, 
            request.Goods);

        var calculationId = await _calculationService.SaveCalculation(
            model, 
            cancellationToken);

        return new CalculateDeliveryPriceResult(
            calculationId,
            resultPrice);
    }
}