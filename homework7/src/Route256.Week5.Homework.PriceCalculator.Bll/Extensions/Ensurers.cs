using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Extensions;

public static class Ensurers
{
    public static CalculateDeliveryPriceCommand EnsureHasGoods(
        this CalculateDeliveryPriceCommand src)
    {
        if (!src.Goods.Any())
        {
            throw new GoodsNotFoundException();
        }

        return src;
    }
}