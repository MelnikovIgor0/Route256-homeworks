using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Route256.PriceCalculator.Domain;
using Route256.PriceCalculator.Domain.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Services;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;
using Xunit;
using Route256.PriceCalculator.Infrastructure.Dal.Repositories;

namespace Workshop.UnitTests;

public class PriceCalculatorServiceTests
{
    [Fact]
    public void PriceCalculatorService_WhenGoodsEmptyArray_ShouldThrow()
    {
        // Arrange
        var options = new PriceCalculatorOptions();
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Default);
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var goods = Array.Empty<GoodModel>();

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cut.CalculatePrice(goods));
    }

    [Fact]
    public void PriceCalculatorService_WhenNoQueriesCheckLogSize_ShouldSuccess()
    {
        // Arrange
        var options = new PriceCalculatorOptions { };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        // Assert
        Assert.Equal(cut.QueryLog(0).Length, 0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void PriceCalculatorService_ThereAreFewQueriesGetLog_ShouldSuccess(
        int take)
    {
        // Arrange
        var options = new PriceCalculatorOptions { };

        IStorageRepository repository = new StorageRepository();
        List<StorageEntity> data = new List<StorageEntity>
        {
            new StorageEntity(DateTime.UtcNow, 1, 10, 10, 5),
            new StorageEntity(DateTime.UtcNow, 3, 15, 2, 8),
            new StorageEntity(DateTime.UtcNow, 2, 8, 3, 77)
        };
        for (int i = 0; i < 3; ++i)
        {
            repository.Save(data[i]);
        }
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repository);
        List<CalculationLogModel> expected = new List<CalculationLogModel>();
        for (int i = 2; i > 2 - take; --i)
        {
            expected.Add(new CalculationLogModel(data[i].Volume, data[i].Weight, data[i].Distance, data[i].Price));
        }
        var result = cut.QueryLog(take);
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PriceCalculatorService_WhenCalcAny_ShouldSave()
    {
        StorageEntity storageEntity = null;

        // Arrange
        var options = new PriceCalculatorOptions { VolumeToPriceRatio = 1, WeightToPriceRatio = 1 };
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock
            .Setup(x => x.Save(It.IsAny<StorageEntity>()))
            .Callback<StorageEntity>(x => storageEntity = x);
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var goods = new Fixture().CreateMany<GoodModel>().ToArray();

        // Act
        var result = cut.CalculatePrice(goods);

        // Assert
        Assert.NotNull(storageEntity);
        repositoryMock.Verify(x => x.Save(It.IsAny<StorageEntity>()));
        repositoryMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(0, 0)]
    public void PriceCalculatorService_WhenCalcByVolume_ShouldSuccess(
        decimal volumeToPriceRatio,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(10, 10, 10, 0);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(0, 0)]
    public void PriceCalculatorService_WhenCalcByWeight_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(0, 0, 0, 1000);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1, 10, 10, 10, 2000, 2)]
    [InlineData(2, 1, 10, 10, 10, 2000, 4)]
    [InlineData(1, 2, 10, 9, 10, 2000, 2)]
    public void PriceCalculatorService_WhenCalcByWeightEvaluatesMoreThanCalcByVolume_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal volumeToPriceRatio,
        int height,
        int length,
        int width,
        int weight,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio,
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(height, length, width, weight);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1, 10, 10, 10, 500, 1)]
    [InlineData(1, 2.5, 10, 10, 10, 2499, 2.5)]
    public void PriceCalculatorService_WhenCalcByWeightEvaluatesLessThanCalcByVolume_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal volumeToPriceRatio,
        int height,
        int length,
        int width,
        int weight,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio,
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(height, length, width, weight);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1, 10, 10, 10, 1000, 1)]
    [InlineData(1, 2.5, 10, 10, 10, 2500, 2.5)]
    [InlineData(1, 100, 0, 0, 0, 0, 0)]
    public void PriceCalculatorService_WhenCalcByWeightEvaluatesEqualCalcByVolume_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal volumeToPriceRatio,
        int height,
        int length,
        int width,
        int weight,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio,
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(height, length, width, weight);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1, 9)]
    [InlineData(2, 1, 9)]
    [InlineData(0, 0, 0)]
    [InlineData(1, 2, 18)]
    public void PriceCalculatorService_WhenThereAreFewGoodsBuyed1_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal volumeToPriceRatio,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio,
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        
        // Act
        var result = cut.CalculatePrice(new List<GoodModel> { 
            new GoodModel(10, 10, 10, 1000),
            new GoodModel(20, 20, 20, 1000)
        }.AsReadOnly());

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 1, 10)]
    [InlineData(2, 1, 12)]
    [InlineData(1, 2, 20)]
    public void PriceCalculatorService_WhenThereAreFewGoodsBuyed2_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal volumeToPriceRatio,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio,
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);

        // Act
        var result = cut.CalculatePrice(new List<GoodModel> {
            new GoodModel(10, 10, 10, 0),
            new GoodModel(20, 20, 20, 1000),
            new GoodModel(10, 20, 10, 5000)
        }.AsReadOnly());

        // Assert
        Assert.Equal(expected, result);
    }

    private static Mock<IStorageRepository> CreateRepositoryMock()
    {
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<StorageEntity>()));
        return repositoryMock;
    }
    
    private static PriceCalculatorOptions CreateOptionsSnapshot(
        PriceCalculatorOptions options)
    {
        var repositoryMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);
        
        repositoryMock
            .Setup(x => x.Value)
            .Returns(() => options);

        return repositoryMock.Object.Value;
    }

    [Theory]
    [MemberData(nameof(CalcByVolumeManyMemberData))]
    public void PriceCalculatorService_WhenCalcByVolumeMany_ShouldSuccess(
        GoodModel[] goods,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1, 
        };
        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(CreateOptionsSnapshot(options), repositoryMock.Object);
        
        // Act
        var result = cut.CalculatePrice(goods);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> CalcByVolumeManyMemberData => CalcByVolumeMany();
    private static IEnumerable<object[]> CalcByVolumeMany()
    {
        yield return new object[]
        {
            new GoodModel[] { new(10, 10, 10, 0), }, 1
        };

        yield return new object[]
        {
            Enumerable
                .Range(1, 2)
                .Select(x => new GoodModel(10, 10, 10, 0))
                .ToArray(),
            2
        };
    }
}