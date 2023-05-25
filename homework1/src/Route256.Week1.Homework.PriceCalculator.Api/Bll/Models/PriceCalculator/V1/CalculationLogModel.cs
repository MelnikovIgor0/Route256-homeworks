namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.V1.PriceCalculator;

public record CalculationLogModel(
    decimal Volume,
    decimal Weight,
    decimal Price);