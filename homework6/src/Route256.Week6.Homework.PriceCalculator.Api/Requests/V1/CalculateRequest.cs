namespace Route256.Week5.Workshop.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Товары. чью цену транспортировки нужно расчитать
/// </summary>
public record CalculateRequest(
    long UserId,
    CalculateRequest.GoodProperties[] Goods)
{
    public record GoodProperties(
        double Height,
        double Length,
        double Width,
        double Weight);
}