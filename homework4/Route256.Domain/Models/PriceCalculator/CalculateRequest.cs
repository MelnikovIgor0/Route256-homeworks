namespace Route256.Domain.Models.PriceCalculator;

public sealed record CalculateRequest(GoodModel[] Goods, decimal Distance);