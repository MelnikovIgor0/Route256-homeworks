using System.Transactions;
using Route256.Week5.Workshop.PriceCalculator.Dal.Entities;
using Route256.Week5.Workshop.PriceCalculator.Dal.Models;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices;

public class CalculationRepositoryPlug : ICalculationRepository
{
    public async Task<long[]> Add(CalculationEntityV1[] entityV1, CancellationToken token)
    {
        return Array.Empty<long>();
    }

    public async Task<CalculationEntityV1[]> Query(CalculationHistoryQueryModel query, CancellationToken token)
    {
        return Array.Empty<CalculationEntityV1>();
    }

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
}