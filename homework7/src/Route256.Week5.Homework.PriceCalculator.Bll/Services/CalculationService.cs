using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Services;

public class CalculationService : ICalculationService
{
    public const decimal VolumeToPriceRatio = 3.27m;
    public const decimal WeightToPriceRatio = 1.34m;

    private readonly ICalculationRepository _calculationRepository;
    private readonly IGoodsRepository _goodsRepository;

    public CalculationService(
        ICalculationRepository calculationRepository,
        IGoodsRepository goodsRepository)
    {
        _calculationRepository = calculationRepository;
        _goodsRepository = goodsRepository;
    }

    public async Task<long> SaveCalculation(
        SaveCalculationModel data,
        CancellationToken cancellationToken)
    {
        var goods = data.Goods
            .Select(x => new GoodEntityV1
            {
                UserId = data.UserId,
                Height = x.Height,
                Weight = x.Weight,
                Length = x.Length,
                Width = x.Width
            })
            .ToArray();

        var calculation = new CalculationEntityV1
        {
            UserId = data.UserId,
            TotalVolume = data.TotalVolume,
            TotalWeight = data.TotalWeight,
            Price = data.Price,
            At = DateTimeOffset.UtcNow
        };

        using var transaction = _calculationRepository.CreateTransactionScope();
        var goodIds = await _goodsRepository.Add(goods, cancellationToken);

        calculation = calculation with {GoodIds = goodIds};
        var calculationIds = await _calculationRepository.Add(new[] {calculation}, cancellationToken);
        transaction.Complete();

        return calculationIds.Single();
    }

    public decimal CalculatePriceByVolume(
        GoodModel[] goods,
        out double volume)
    {
        volume = goods
            .Sum(x => x.Length * x.Width * x.Height);

        return (decimal) volume * VolumeToPriceRatio;
    }

    public decimal CalculatePriceByWeight(
        GoodModel[] goods,
        out double weight)
    {
        weight = goods
            .Sum(x => x.Weight);

        return (decimal) weight * WeightToPriceRatio;
    }

    public async Task<QueryCalculationModel[]> QueryCalculations(
        QueryCalculationFilter query,
        CancellationToken token)
    {
        var result = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(
                query.UserId,
                query.Limit,
                query.Offset,
                query.CalculationIds
            ),
            token);

        return result
            .Select(x => new QueryCalculationModel(
                x.Id,
                x.UserId,
                x.TotalVolume,
                x.TotalWeight,
                x.Price,
                x.GoodIds))
            .ToArray();
    }

    /// <summary>
    /// Получить расчеты по идентификаторам
    /// </summary>
    /// <param name="calculationIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QueryCalculationModel[]> GetCalculation(long[] calculationIds,
        CancellationToken cancellationToken)
    {
        var result = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(
                UserId: null,
                Limit: calculationIds.Length,
                Offset: 0,
                CalculationIds: calculationIds),
            cancellationToken
        );
        return result
            .Select(x => new QueryCalculationModel(
                x.Id,
                x.UserId,
                x.TotalVolume,
                x.TotalWeight,
                x.Price,
                x.GoodIds))
            .ToArray();
    }

    /// <summary>
    /// Удалить расчеты пользователя и товары 
    /// </summary>
    /// <param name="goodsIds">список товаров</param>
    /// <param name="calculationIds">список расчетов</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OneOrManyCalculationsBelongsToAnotherUserException">пользователь не явлется держателем данных</exception>
    /// <exception cref="OneOrManyCalculationsNotFoundException">некоторые расчеты не найдены</exception>
    public async Task DeleteCalculationAndGoods(
        long[] calculationIds,
        long[] goodsIds,
        CancellationToken cancellationToken
    )
    {
        using var transaction = _calculationRepository.CreateTransactionScope();

        await _calculationRepository.Delete(
            calculationIds,
            cancellationToken);

        await _goodsRepository.Delete(
            goodsIds,
            cancellationToken);

        transaction.Complete();
    }

}