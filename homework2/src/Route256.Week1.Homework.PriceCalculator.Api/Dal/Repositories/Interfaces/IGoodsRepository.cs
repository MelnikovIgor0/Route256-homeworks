using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodEntity entity);
    
    ICollection<GoodEntity> GetAll();
    GoodEntity Get(int id);
}