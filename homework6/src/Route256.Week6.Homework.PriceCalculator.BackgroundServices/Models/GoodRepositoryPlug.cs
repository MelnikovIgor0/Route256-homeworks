using System.Transactions;
using Route256.Week5.Workshop.PriceCalculator.Dal.Entities;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices.Models;

public class GoodRepositoryPlug : IGoodsRepository
{
    public TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted)
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

    public async Task<long[]> Add(GoodEntityV1[] goods, CancellationToken token)
    {
        return Array.Empty<long>();
    }

    public async Task<GoodEntityV1[]> Query(long userId, CancellationToken token)
    {
        return Array.Empty<GoodEntityV1>();
    }
}