namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

public class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public OneOrManyCalculationsBelongsToAnotherUserException() : base("\r\nOne or more calculations belong to another user")
    {
    }
}
