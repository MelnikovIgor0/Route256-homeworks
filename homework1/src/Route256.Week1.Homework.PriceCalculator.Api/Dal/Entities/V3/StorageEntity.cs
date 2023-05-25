namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V3;

public record StorageEntity(
    DateTime At,
    decimal Volume,
    decimal Weight,
    decimal Price,
    decimal Distance);
