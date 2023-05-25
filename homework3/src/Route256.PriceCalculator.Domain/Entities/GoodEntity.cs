namespace Route256.PriceCalculator.Domain.Entities;

public sealed record GoodEntity(
    string Name,
    int Id,
    int Height,
    int Length,
    int Width,
    int Weight,
    int Count,
    decimal Price
);