namespace Route256.Week5.Homework.PriceCalculator.Bll.Models;

public record QueryCalculationFilter(
    long? UserId,
    int Limit,
    int Offset, 
    long[]? CalculationIds
 );