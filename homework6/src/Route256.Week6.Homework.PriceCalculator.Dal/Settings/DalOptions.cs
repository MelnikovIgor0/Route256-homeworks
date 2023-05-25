namespace Route256.Week5.Workshop.PriceCalculator.Dal.Settings;

public record DalOptions
{
    public string ConnectionString { get; init; } = string.Empty;
}