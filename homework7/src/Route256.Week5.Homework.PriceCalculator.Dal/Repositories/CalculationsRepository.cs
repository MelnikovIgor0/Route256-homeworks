using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class CalculationRepository : BaseRepository, ICalculationRepository
{
    public CalculationRepository() : base()
    {
    }

    public async Task<long[]> Add(
        CalculationEntityV1[] entityV1, 
        CancellationToken token)
    {
        const string sqlQuery = @"
insert into calculations (user_id, good_ids, total_volume, total_weight, price, at)
select user_id, good_ids, total_volume, total_weight, price, at
  from UNNEST(@Calculations)
returning id;
";

        var sqlQueryParams = new
        {
            Calculations = entityV1
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

    public async Task<CalculationEntityV1[]> Query(
        CalculationHistoryQueryModel query,
        CancellationToken token)
    {

        if (query.UserId == null && query.CalculationIds == null)
            throw new ArgumentException($"{nameof(query.UserId)} or {nameof(query.CalculationIds)} necessarily", nameof(query));

        if (query.UserId == null && query.CalculationIds != null && !query.CalculationIds.Any())
            throw new ArgumentException($"{nameof(query.CalculationIds)} shouldn't be empty", nameof(query));

        var conditions = new List<string>();

        if (query.UserId != null)
            conditions.Add("user_id = @UserId");
        if (query.CalculationIds != null && query.CalculationIds.Any())
            conditions.Add("id = any(@CalculationIds)");

        var sqlQuery = @$"
 select 
     id,
     user_id,
     good_ids,
     total_volume,
     total_weight,
     price,
     at
 from calculations
 where {string.Join(" and ", conditions)}
 order by at desc
 limit @Limit offset @Offset
";

        var sqlQueryParams = new
        {
            UserId = query.UserId,
            CalculationIds = query.CalculationIds,
            Limit = query.Limit,
            Offset = query.Offset
        };

        await using var connection = await GetAndOpenConnection();
        var calculations = await connection.QueryAsync<CalculationEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        
        return calculations
            .ToArray();
    }

    public async Task Delete(long[] calculationIds, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
 delete from calculations where id = any(@CalculationIds);
";
        await using var connection = await GetAndOpenConnection();

        var sqlQueryParams = new
        {
            CalculationIds = calculationIds
        };

        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken));
    }
}