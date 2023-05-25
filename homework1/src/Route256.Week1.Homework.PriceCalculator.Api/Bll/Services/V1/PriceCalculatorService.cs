using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.V1.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V1.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V1;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V1;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const decimal volumeToPriceRatio = 3.27m;
    private const decimal weightToPriceRatio = 1.34m;
    
    private readonly IStorageRepository<StorageEntity> _storageRepository;
    
    public PriceCalculatorService(
        IStorageRepository<StorageEntity> storageRepository)
    {
        _storageRepository = storageRepository;
    }
    
    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods)
    {
        if (!goods.Any())
        {
            throw new ArgumentOutOfRangeException(nameof(goods));
        }

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice);
        
        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            resultPrice));
        
        return resultPrice;
    }

    private decimal CalculatePriceByVolume(
        IReadOnlyList<GoodModel> goods,
        out decimal volume)
    {
        volume = goods
            .Select(x => x.Height * x.Width * x.Height / 1000)
            .Sum();

        return volume * volumeToPriceRatio;
    }
    
    private decimal CalculatePriceByWeight(
        IReadOnlyList<GoodModel> goods,
        out decimal weight)
    {
        weight = goods
            .Select(x => x.Weight / 1000)
            .Sum();

        return weight * weightToPriceRatio;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take == 0)
        {
            return Array.Empty<CalculationLogModel>();
        }
        
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Weight,
                x.Price))
            .ToArray();
    }
}