using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class CalculationRepository : BaseRepository, ICalculationRepository
{
    public CalculationRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
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
        const string sqlQuery = @"
select id
     , user_id
     , good_ids
     , total_volume
     , total_weight
     , price
     , at
  from calculations
 where user_id = @UserId
 order by at desc
 limit @Limit offset @Offset
";
        
        var sqlQueryParams = new
        {
            UserId = query.UserId,
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

    public async Task<IEnumerable<long>> GetInvalidIdsByUser(
        ClearHistoryQueryModel query,
        CancellationToken token)
    {
        if (query.CalculationIds.Length == 0)
        {
            return Array.Empty<long>();
        }
        await using var connection = await GetAndOpenConnection();
        var sqlParams = new
        {
            UserId = query.UserId,
            CalculationsIds = query.CalculationIds
        };
        string sqlQuery = @"SELECT id FROM calculations WHERE id = 
ANY(@CalculationsIds) AND user_id != @UserId";
        IEnumerable<long> invalidIds = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                parameters: sqlParams,
                cancellationToken: token));
        return invalidIds;
    }

    public async Task<bool> AreCalculationIdsInvalid(
        ClearHistoryQueryModel query,
        CancellationToken token)
    {
        if (query.CalculationIds.Length == 0)
        {
            return false;
        }
        await using var connection = await GetAndOpenConnection();
        var sqlParams = new
        {
            NumberCalculations = query.CalculationIds.Length,
            CalculationsIds = query.CalculationIds
        };
        string sqlQuery = @"SELECT COUNT(*) = @NumberCalculations
FROM calculations WHERE id = ANY(@CalculationsIds)";
        IEnumerable<bool> notFoundCalculations = await connection.QueryAsync<bool>(
            new CommandDefinition(
                sqlQuery,
                parameters: sqlParams,
                cancellationToken: token));
        return notFoundCalculations.Any(x => !x);
    }

    public async Task ClearHistoryByEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token)
    {
        await using var connection = await GetAndOpenConnection();
        string sqlQuery = @"DELETE FROM calculations WHERE user_id = @UserId";
        var sqlParams = new { UserId = query.UserId };
        await connection.QueryAsync(
        new CommandDefinition(
            sqlQuery,
            parameters: sqlParams,
            cancellationToken: token));
    }
    
    public async Task ClearHistoryByNonEmptyList(
        ClearHistoryQueryModel query,
        CancellationToken token)
    {
        await using var connection = await GetAndOpenConnection();
        string sqlQuery = @"DELETE FROM calculations WHERE id = ANY(@CalculationsIds)";;
        var sqlParams = new { CalculationsIds = query.CalculationIds };
        await connection.QueryAsync(
        new CommandDefinition(
            sqlQuery,
            parameters: sqlParams,
            cancellationToken: token));
    }
}