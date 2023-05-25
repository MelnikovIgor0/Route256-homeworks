using System.Linq;
using System.Threading;
using Moq;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Comparers;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Extensions;

public static class GoodsRepositoryExtensions
{
    public static Mock<IGoodsRepository> SetupAddGoods(
        this Mock<IGoodsRepository> repository,
        long[] ids)
    {
        repository.Setup(p =>
                p.Add(It.IsAny<GoodEntityV1[]>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ids);

        return repository;
    }

    public static Mock<IGoodsRepository> SetupQueryGoods(
        this Mock<IGoodsRepository> repository,
        GoodEntityV1[] goods)
    {
        repository.Setup(p =>
                p.Query(It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(goods);

        return repository;
    }

    public static void VerifyAddWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        GoodEntityV1[] goods)
    {
        repository.Verify(p =>
                p.Add(
                    It.Is<GoodEntityV1[]>(x => x.SequenceEqual(goods, new GoodEntityV1Comparer())),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public static void VerifyQueryWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        long userId)
    {
        repository.Verify(p =>
                p.Query(
                    It.Is<long>(x => x == userId),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public static Mock<IGoodsRepository> SetupDeleteGoods(
        this Mock<IGoodsRepository> repository)
    {
        repository.Setup(p =>
            p.Delete(It.IsAny<long[]>(),
                It.IsAny<CancellationToken>()));

        return repository;
    }

    public static void VerifyDeleteWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        long[] goodIds)
    {
        repository.Verify(p =>
                p.Delete(
                    It.Is<long[]>(x => x.SequenceEqual(goodIds)),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
}