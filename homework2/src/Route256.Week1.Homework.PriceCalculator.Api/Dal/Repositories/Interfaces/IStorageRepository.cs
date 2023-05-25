using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface IStorageRepository
{
    void Save(StorageEntity entity);

    IReadOnlyList<StorageEntity> Query();
}