using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V1;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.V1;

public class StorageRepository : IStorageRepository<StorageEntity>
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