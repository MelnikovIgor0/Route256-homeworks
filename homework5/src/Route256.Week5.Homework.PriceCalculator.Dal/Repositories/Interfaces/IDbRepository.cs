using System.Transactions;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

public interface IDbRepository
{
    TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted);
}