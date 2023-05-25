using System.Transactions;
using Npgsql;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public abstract class BaseRepository : IDbRepository
{

    protected BaseRepository()
    {
    }
    
    protected async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection("User ID=postgres;Password=123456;Host=localhost;Port=15432;Database=price-calculator;Pooling=true;");
        await connection.OpenAsync();
        connection.ReloadTypes();
        return connection;
    }
    
    public TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions 
            { 
                IsolationLevel = level, 
                Timeout = TimeSpan.FromSeconds(5) 
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}