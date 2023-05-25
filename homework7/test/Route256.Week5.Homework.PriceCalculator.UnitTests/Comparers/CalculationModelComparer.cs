using System;
using System.Collections.Generic;
using System.Linq;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Comparers;

public class CalculationModelComparer : IEqualityComparer<SaveCalculationModel>
{
    private const double tolerance = 1e-8;
    
    public bool Equals(SaveCalculationModel? x, SaveCalculationModel? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if ((x is null && y is not null) || (x is not null && y is null))
        {
            return false;
        }

        return x!.UserId == y!.UserId
               && x.Price == y.Price
               && Math.Abs(x.TotalVolume - y.TotalVolume) < tolerance
               && Math.Abs(x.TotalWeight - y.TotalWeight) < tolerance
               && x.Goods.SequenceEqual(y.Goods);
    }

    public int GetHashCode(SaveCalculationModel obj)
    {
        return HashCode.Combine(obj.UserId, 
            obj.Price, 
            obj.TotalVolume, 
            obj.TotalWeight,
            obj.Goods);
    }
}
