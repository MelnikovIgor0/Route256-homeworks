using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;

namespace Route256.PriceCalculator.Infrastructure.Dal.Repositories;

internal sealed class GoodsRepository : IGoodsRepository
{
    private readonly Dictionary<int, GoodEntity> _store = new Dictionary<int, GoodEntity>();

    public void AddOrUpdate(GoodEntity entity)
    {
        if (_store.ContainsKey(entity.Id))
            _store.Remove(entity.Id);
        
        _store.Add(entity.Id, entity);
    }

    public ICollection<GoodEntity> GetAll()
    {
        return _store.Select(x => x.Value).ToArray();
    }

    public GoodEntity Get(int id) => _store[id];
}