namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class GoodsNotFoundException : Exception
{
    public GoodsNotFoundException() : base("Товары не найдены")
    {
        
    }
}