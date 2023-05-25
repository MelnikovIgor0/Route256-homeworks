using System.Collections.Generic;
using System.Linq;
using AutoBogus;
using Bogus;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;

namespace Route256.Week5.Workshop.PriceCalculator.UnitTests.Fakers;

public static class CalculationModelFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<SaveCalculationModel> Faker = new AutoFaker<SaveCalculationModel>()
        .RuleFor(x => x.Price, f => f.Random.Decimal())
        .RuleFor(x => x.TotalVolume, f => f.Random.Double())
        .RuleFor(x => x.TotalWeight, f => f.Random.Double());

    public static IEnumerable<SaveCalculationModel> Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable.Repeat(Faker.Generate(), count);    
        }
    }

    public static SaveCalculationModel WithUserId(
        this SaveCalculationModel src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static SaveCalculationModel WithGoods(
        this SaveCalculationModel src, 
        GoodModel[] goods)
    {
        return src with { Goods = goods };
    }
}