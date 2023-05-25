namespace Route256.Week5.Workshop.PriceCalculator.Dal.Entities;

public record CalculationEntityV1
{
    public long Id { get; init; }
    
    public long UserId { get; init; }
    
    public long[] GoodIds { get; init; } = Array.Empty<long>();
    
    public double TotalVolume { get; init; }
    
    public double TotalWeight { get; init; }
    
    public decimal Price { get; init; }
    
    public DateTimeOffset At { get; init; }
}
