namespace Route256.PriceCalculator.Domain.Models.PriceCalculator;

public sealed record CalculateRequest(GoodModel[] Goods, decimal Distance);