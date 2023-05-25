using System;
using System.Collections.Generic;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Comparers;

public class CalculationEntityV1Comparer : IEqualityComparer<CalculationEntityV1>
{
    public bool Equals(CalculationEntityV1? x, CalculationEntityV1? y)
    {
        return x?.UserId == y?.UserId
               && x?.Price == y?.Price
               && x?.TotalVolume == y?.TotalVolume
               && x?.TotalWeight == y?.TotalWeight;
    }

    public int GetHashCode(CalculationEntityV1 obj)
    {
        return HashCode.Combine(obj.UserId, obj.Price, obj.TotalVolume, obj.TotalWeight);
    }
}
