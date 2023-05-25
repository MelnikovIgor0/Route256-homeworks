namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

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