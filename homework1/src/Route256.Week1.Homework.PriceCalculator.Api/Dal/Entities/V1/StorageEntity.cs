namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V1;

public record StorageEntity(
    DateTime At,
    decimal Volume,
    decimal Weight,
    decimal Price);
