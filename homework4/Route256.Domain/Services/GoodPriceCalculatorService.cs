using Route256.Domain.Entities;
using Route256.Domain.Models.PriceCalculator;
using Route256.Domain.Separated;
using Route256.Domain.Services.Interfaces;

namespace Route256.Domain.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _repository;
    private readonly IPriceCalculatorService _service;

    public GoodPriceCalculatorService(
        IGoodsRepository repository,
        IPriceCalculatorService service)
    {
        _repository = repository;
        _service = service;
    }

    public decimal CalculatePrice(int goodId, decimal distance)
    {
        if (goodId == default)
            throw new ArgumentException($"{nameof(goodId)} is default");

        if (distance == default)
            throw new ArgumentException($"{nameof(distance)} is default");

        var currentGood = _repository.Get(goodId);
        var currentModel = new GoodModel(1, currentGood.Height,
            currentGood.Length,
            currentGood.Width,
            currentGood.Weight);
        var modelsArray = new[] { currentModel };

        return _service.CalculatePrice(modelsArray) * distance;
    }
}