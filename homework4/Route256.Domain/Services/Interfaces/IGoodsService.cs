using Route256.Domain.Entities;

namespace Route256.Domain.Services.Interfaces;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}