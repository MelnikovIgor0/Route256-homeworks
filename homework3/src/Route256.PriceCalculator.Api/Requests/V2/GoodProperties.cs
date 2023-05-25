namespace Route256.PriceCalculator.Api.Requests.V2;

/// <summary>
/// Харектеристики товара
/// </summary>
public record GoodProperties(
    int Height,
    int Length,
    int Width,
    int Weight);