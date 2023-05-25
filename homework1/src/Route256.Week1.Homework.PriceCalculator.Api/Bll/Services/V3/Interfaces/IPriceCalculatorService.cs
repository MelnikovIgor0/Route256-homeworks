using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.V3.PriceCalculator;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V3.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, decimal distance);

    CalculationLogModel[] QueryLog(int take);
}