using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class GoodsRepository : BaseRepository, IGoodsRepository
{
    public GoodsRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }
    
    public async Task<long[]> Add(
        GoodEntityV1[] goods, 
        CancellationToken token)
    {
        const string sqlQuery = @"
insert into goods (user_id, width, height, length, weight) 
select user_id, width, height, length, weight
  from UNNEST(@Goods)
returning id;
";
        
        var sqlQueryParams = new
        {
            Goods = goods
        };
        
        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        
        return ids
            .ToArray();
    }

    public async Task<GoodEntityV1[]> Query(
        long userId,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id
     , user_id
     , width
     , height
     , length
     , weight
  from goods
 where user_id = @UserId;
";
        
        var sqlQueryParams = new
        {
            UserId = userId
        };

        await using var connection = await GetAndOpenConnection();
        var goods = await connection.QueryAsync<GoodEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        
        return goods
            .ToArray();
    }
    
    public async Task ClearGoodsByEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token
    )
    {
        await using var connection = await GetAndOpenConnection();
        var sqlQuery = @"DELETE FROM goods WHERE user_id = @UserId";
        var sqlParams = new { UserId = query.UserId,};
        
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                parameters: sqlParams,
                cancellationToken: token));
    }
    
    public async Task ClearGoodsByNonEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token
    )
    {
        await using var connection = await GetAndOpenConnection();
        
        var sqlQuery = @"DELETE FROM goods WHERE id IN (SELECT UNNEST(good_ids) FROM 
calculations WHERE id = ANY(@CalculationsIds))";
        var sqlParams = new { CalculationsIds = query.CalculationIds };
        await connection.QueryAsync(
        new CommandDefinition(
            sqlQuery,
            parameters: sqlParams,
            cancellationToken: token));
    }
}