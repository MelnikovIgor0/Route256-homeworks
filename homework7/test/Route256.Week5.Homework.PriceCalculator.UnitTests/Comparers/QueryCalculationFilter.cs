using System;
using System.Collections.Generic;
using System.Linq;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Comparers;

public class QueryCalculationFilterComparer : IEqualityComparer<QueryCalculationFilter>
{
    public bool Equals(QueryCalculationFilter? x, QueryCalculationFilter? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;

        return x.UserId == y.UserId
               && x.Limit == y.Limit
               && x.Offset == y.Offset
               && (
                   x.CalculationIds == null && y.CalculationIds == null
                   || x.CalculationIds != null
                   && y.CalculationIds != null
                   && x.CalculationIds.SequenceEqual(y.CalculationIds)
               );
    }

    public int GetHashCode(QueryCalculationFilter obj)
    {
        return HashCode.Combine(obj.UserId, obj.Limit, obj.Offset, obj.CalculationIds);
    }
}
