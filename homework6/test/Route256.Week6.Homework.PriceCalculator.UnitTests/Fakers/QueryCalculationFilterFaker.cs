using AutoBogus;
using Bogus;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;

namespace Route256.Week5.Workshop.PriceCalculator.UnitTests.Fakers;

public static class QueryCalculationFilterFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<QueryCalculationFilter> Faker = new AutoFaker<QueryCalculationFilter>()
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Limit, f => f.Random.Int(1, 5))
        .RuleFor(x => x.Offset, f => f.Random.Int(0, 5));
    
    public static QueryCalculationFilter Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
    
    public static QueryCalculationFilter WithUserId(
        this QueryCalculationFilter src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static QueryCalculationFilter WithLimit(
        this QueryCalculationFilter src, 
        int limit)
    {
        return src with { Limit = limit };
    }
    
    public static QueryCalculationFilter WithOffset(
        this QueryCalculationFilter src, 
        int offset)
    {
        return src with { Offset = offset };
    }
}