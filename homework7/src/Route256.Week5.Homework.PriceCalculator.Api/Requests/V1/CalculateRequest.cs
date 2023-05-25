namespace Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Товары, цену транспортировки которых нужно рассчитать
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