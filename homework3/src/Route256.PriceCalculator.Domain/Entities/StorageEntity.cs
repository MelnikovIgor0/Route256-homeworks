namespace Route256.PriceCalculator.Domain.Entities;

public record StorageEntity(
    DateTime At,
    decimal Volume,
    decimal Weight,
    decimal Distance,
    decimal Price);
