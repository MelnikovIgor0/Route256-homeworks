namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsNotFoundException : Exception
{
    public OneOrManyCalculationsNotFoundException() : base("One or more calculations not found")
    {
    }
}