namespace Route256.Week5.Homework.PriceCalculator.Dal.Models;

public record CalculationHistoryQueryModel(
    long? UserId,
    int Limit,
    int Offset,
    long[]? CalculationIds = null
    );