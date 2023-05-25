using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

public interface IGoodsRepository : IDbRepository
{
    Task<long[]> Add(
        GoodEntityV1[] goods, 
        CancellationToken token);

    Task<GoodEntityV1[]> Query(
        long userId,        
        CancellationToken token);
    
    Task ClearGoodsByEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token
    );

    Task ClearGoodsByNonEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token
    );
}