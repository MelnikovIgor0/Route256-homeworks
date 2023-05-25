namespace Route256.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Харектеристики товара
/// </summary>
public record GoodProperties(
    int Height,
    int Length,
    int Width);