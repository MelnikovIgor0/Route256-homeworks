using AutoBogus;
using Bogus;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Fakers;

public static class GetCalculationHistoryQueryFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<GetCalculationHistoryQuery> Faker = new AutoFaker<GetCalculationHistoryQuery>()
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Take, f => f.Random.Int(1, 5))
        .RuleFor(x => x.Skip, f => f.Random.Int(0, 5));
    
    public static GetCalculationHistoryQuery Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
    
    public static GetCalculationHistoryQuery WithUserId(
        this GetCalculationHistoryQuery src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static GetCalculationHistoryQuery WithLimit(
        this GetCalculationHistoryQuery src, 
        int take)
    {
        return src with { Take = take };
    }
    
    public static GetCalculationHistoryQuery WithOffset(
        this GetCalculationHistoryQuery src, 
        int skip)
    {
        return src with { Skip = skip };
    }
}