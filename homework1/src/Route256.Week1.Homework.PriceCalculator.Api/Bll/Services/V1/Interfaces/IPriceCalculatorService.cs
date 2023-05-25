using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.V1.PriceCalculator;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V1.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);

    CalculationLogModel[] QueryLog(int take);
}