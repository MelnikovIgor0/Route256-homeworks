namespace Route256.PriceCalculator.Api.Requests.V2;

/// <summary>
/// Товары. чью цену транспортировки нужно расчитать
/// </summary>
public record CalculateRequest(
    GoodProperties[] Goods
    );