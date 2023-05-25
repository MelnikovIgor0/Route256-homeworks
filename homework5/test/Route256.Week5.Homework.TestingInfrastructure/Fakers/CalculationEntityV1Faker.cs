using AutoBogus;
using Bogus;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week5.Homework.TestingInfrastructure.Fakers;

public static class CalculationEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<CalculationEntityV1> Faker = new AutoFaker<CalculationEntityV1>()
        .RuleFor(x => x.Id, f => f.Random.Long(0L))
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Price, f => f.Random.Decimal())
        .RuleFor(x => x.TotalVolume, f => f.Random.Double())
        .RuleFor(x => x.TotalWeight, f => f.Random.Double());

    public static CalculationEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable.Repeat(Faker.Generate(), count)
                .ToArray();
        }
    }

    public static CalculationEntityV1 WithId(
        this CalculationEntityV1 src, 
        long id)
    {
        return src with { Id = id };
    }
    
    public static CalculationEntityV1 WithUserId(
        this CalculationEntityV1 src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static CalculationEntityV1 WithTotalVolume(
        this CalculationEntityV1 src, 
        double totalVolume)
    {
        return src with { TotalVolume = totalVolume };
    }
    
    public static CalculationEntityV1 WithTotalWeight(
        this CalculationEntityV1 src, 
        double totalWeight)
    {
        return src with { TotalWeight = totalWeight };
    }
    
    public static CalculationEntityV1 WithPrice(
        this CalculationEntityV1 src, 
        decimal price)
    {
        return src with { Price = price };
    }
    
    public static CalculationEntityV1 WithAt(
        this CalculationEntityV1 src, 
        DateTimeOffset at)
    {
        return src with { At = at };
    }
}