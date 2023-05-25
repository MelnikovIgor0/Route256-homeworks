using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;

namespace Route256.PriceCalculator.Infrastructure.Dal.Repositories;

internal sealed class StorageRepository : IStorageRepository
{
    private readonly List<StorageEntity> _store;

    public StorageRepository()
    {
        _store = new List<StorageEntity>();
    }
    
    public void Save(StorageEntity entity)
    {
        _store.Add(entity);
    }

    public IReadOnlyList<StorageEntity> Query()
    {
        return _store;
    }
}