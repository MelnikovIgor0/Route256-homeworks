using Route256.PriceCalculator.Domain.Entities;

namespace Route256.PriceCalculator.Domain.Separated;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodEntity entity);
    
    ICollection<GoodEntity> GetAll();
    GoodEntity Get(int id);
}