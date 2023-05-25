using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface ITotalPriceCalculatorService
{
    decimal CalculateTotalPrice(GoodEntity good);
}
