namespace Route256.PriceCalculator.Domain.Models.PriceCalculator;

public record CalculationLogModel(
    decimal Volume,
    decimal Weight,
    decimal Distance,
    decimal Price);