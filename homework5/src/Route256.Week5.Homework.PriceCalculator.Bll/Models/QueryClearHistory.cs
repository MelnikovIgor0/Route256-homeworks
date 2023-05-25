namespace Route256.Week5.Homework.PriceCalculator.Bll.Models;

public record QueryClearHistory (
    long UserId,
    long[] CalculationIds);