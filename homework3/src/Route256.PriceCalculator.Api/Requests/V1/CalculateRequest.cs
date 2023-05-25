namespace Route256.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Товары. чью цену транспортировки нужно расчитать
/// </summary>
public record CalculateRequest(
    GoodProperties[] Goods
    );