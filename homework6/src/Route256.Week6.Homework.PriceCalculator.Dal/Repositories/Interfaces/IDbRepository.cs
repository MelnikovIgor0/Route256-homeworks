using System.Transactions;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;

public interface IDbRepository
{
    TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
}