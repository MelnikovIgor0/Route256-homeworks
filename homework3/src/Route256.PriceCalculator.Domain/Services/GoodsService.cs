using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Services.Interfaces;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class GoodsService : IGoodsService
{
    private readonly List<GoodModel> _goods = new()
    {
        new("Парик для питомца", 1, 1000, 2000, 3000, 4000, 100),
        new("Накидка на телевизор", 2, 1000, 2000, 3000, 4000, 120),
        new("Ковёр настенный", 3, 2000, 3000, 3000, 5000, 140),
        new("Здоровенный ЯЗЬ", 4, 1000, 1000, 4000, 4000, 160),
        new("Билет МММ", 5, 3000, 2000, 1000, 5000, 180)
    };
    
    public IEnumerable<GoodEntity> GetGoods()
    {
        var rnd = new Random();
        foreach (var model in _goods)
        {
            var count = rnd.Next(0, 10);
            yield return new GoodEntity(
                model.Name,
                model.Id,
                model.Height,
                model.Length,
                model.Width,
                model.Weight,
                count,
                model.Price
            );
        }
    }

    private record GoodModel(
        string Name,
        int Id,
        int Height,
        int Length,
        int Width,
        int Weight,
        decimal Price);
}