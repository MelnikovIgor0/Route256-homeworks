namespace Route256.PriceCalculator.Domain.Services;

public interface IGoodPriceCalculatorService
{
    decimal CalculatePrice(
        int goodId, 
        decimal distance);
}