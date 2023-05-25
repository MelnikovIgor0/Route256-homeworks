using Route256.Domain.Entities;

namespace Route256.Domain.Separated;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodEntity entity);

    ICollection<GoodEntity> GetAll();
    GoodEntity Get(int id);
}