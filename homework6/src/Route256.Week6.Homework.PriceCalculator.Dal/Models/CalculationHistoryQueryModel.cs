namespace Route256.Week5.Workshop.PriceCalculator.Dal.Models;

public record CalculationHistoryQueryModel(
    long UserId,
    int Limit,
    int Offset);