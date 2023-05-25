namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.V3.PriceCalculator;

public record CalculationLogModel(
    decimal Volume,
    decimal Weight,
    decimal Price,
    decimal Distance);