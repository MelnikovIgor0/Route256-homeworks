using Route256.PriceCalculator.Domain.Entities;

namespace Route256.PriceCalculator.Domain.Separated;

public interface IStorageRepository
{
    void Save(StorageEntity entity);

    IReadOnlyList<StorageEntity> Query();
}