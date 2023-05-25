using AutoBogus;
using Bogus;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Fakers;

public static class CalculationHistoryQueryModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<CalculationHistoryQueryModel> Faker = new AutoFaker<CalculationHistoryQueryModel>()
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Limit, f => f.Random.Int(1, 5))
        .RuleFor(x => x.Offset, f => f.Random.Int(0, 5));
 
    public static CalculationHistoryQueryModel Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
    
    public static CalculationHistoryQueryModel WithUserId(
        this CalculationHistoryQueryModel src, 
        long? userId)
    {
        return src with { UserId = userId };
    }
    
    public static CalculationHistoryQueryModel WithLimit(
        this CalculationHistoryQueryModel src, 
        int limit)
    {
        return src with { Limit = limit };
    }
    
    public static CalculationHistoryQueryModel WithOffset(
        this CalculationHistoryQueryModel src, 
        int offset)
    {
        return src with { Offset = offset };
    }

    public static CalculationHistoryQueryModel WithIds(
        this CalculationHistoryQueryModel src,
        long[]? Ids)
    {
        return src with { CalculationIds = Ids };
    }
}