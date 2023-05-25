namespace Route256.Week5.Workshop.PriceCalculator.Bll.Models;

public record QueryCalculationFilter(
    long UserId,
    int Limit,
    int Offset);