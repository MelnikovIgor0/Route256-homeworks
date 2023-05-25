namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

public record StorageEntity(
    DateTime At,
    decimal Volume,
    decimal Weight,
    decimal Price);
