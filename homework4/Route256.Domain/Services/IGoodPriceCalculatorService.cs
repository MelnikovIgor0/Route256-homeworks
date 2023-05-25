namespace Route256.Domain.Services;

public interface IGoodPriceCalculatorService
{
    decimal CalculatePrice(
        int goodId,
        decimal distance);
}