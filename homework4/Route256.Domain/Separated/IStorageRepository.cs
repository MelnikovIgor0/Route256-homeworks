using Route256.Domain.Entities;

namespace Route256.Domain.Separated;

public interface IStorageRepository
{
    void Save(StorageEntity entity);

    IReadOnlyList<StorageEntity> Query();
}