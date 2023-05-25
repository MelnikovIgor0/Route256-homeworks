using Route256.Domain.Models.PriceCalculator;

namespace Route256.Domain.Services.Interfaces;

public interface IPriceCalculatorService
{
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);
    decimal CalculatePrice(CalculateRequest request);
}