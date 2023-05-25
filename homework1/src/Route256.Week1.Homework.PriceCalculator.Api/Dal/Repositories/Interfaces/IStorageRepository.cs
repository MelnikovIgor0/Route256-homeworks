namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface IStorageRepository<StorageEntity>
{
    void Save(StorageEntity entity);

    IReadOnlyList<StorageEntity> Query();
}