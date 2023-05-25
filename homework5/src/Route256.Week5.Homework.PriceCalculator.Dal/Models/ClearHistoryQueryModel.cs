namespace Route256.Week5.Homework.PriceCalculator.Dal.Models;

public record ClearHistoryQueryModel(
    long UserId,
    long[] CalculationIds);