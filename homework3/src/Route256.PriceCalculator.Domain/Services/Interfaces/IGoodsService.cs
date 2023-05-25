using Route256.PriceCalculator.Domain.Entities;

namespace Route256.PriceCalculator.Domain.Services.Interfaces;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}