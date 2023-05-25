using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class TotalPriceCalculatorService : ITotalPriceCalculatorService
{
    IPriceCalculatorService _deliveryCalculator;

    public TotalPriceCalculatorService(IPriceCalculatorService deliveryCalculator) =>
        _deliveryCalculator = deliveryCalculator;

    public decimal CalculateTotalPrice(GoodEntity good)
    {
        IReadOnlyList<GoodModel> goods = new List<GoodModel> { 
            new GoodModel(good.Height, good.Length, good.Width, good.Weight) 
        }.AsReadOnly();
        return good.Price + _deliveryCalculator.CalculatePrice(goods);
    }
}
