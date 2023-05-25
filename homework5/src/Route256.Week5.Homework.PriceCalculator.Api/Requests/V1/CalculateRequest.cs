namespace Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Товары. чью цену транспортировки нужно расчитать
/// </summary>
public record CalculateRequest(
    long UserId,
    GoodProperties[] Goods);
