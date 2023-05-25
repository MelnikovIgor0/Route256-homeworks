using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Builders;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Extensions;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Fakers;
using Route256.Week5.Homework.TestingInfrastructure.Creators;
using Xunit;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.HandlersTests;

public class CalculateDeliveryPriceCommandHandlerTests
{
    [Fact]
    public async Task Handle_MakeAllCalls()
    {
        //arrange
        var userId = Create.RandomId();
        var calculationId = Create.RandomId();

        var command = CalculateDeliveryPriceCommandFaker.Generate()
            .WithUserId(userId);

        var calculationModel = CalculationModelFaker.Generate()
            .Single()
            .WithUserId(userId)
            .WithGoods(command.Goods);

        var builder = new CalculateDeliveryPriceHandlerBuilder();
        builder.CalculationService
            .SetupCalculatePriceByWeight(calculationModel.TotalWeight, calculationModel.Price)
            .SetupCalculatePriceByVolume(calculationModel.TotalVolume, calculationModel.Price)
            .SetupSaveCalculation(calculationId);

        var handler = builder.Build();
        
        //act
        var result = await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifySaveCalculationWasCalledOnce(calculationModel)
            .VerifyCalculatePriceByVolumeWasCalledOnce(calculationModel.Goods)
            .VerifyCalculatePriceByWeightWasCalledOnce(calculationModel.Goods);
        
        handler.VerifyNoOtherCalls();

        result.CalculationId.Should().Be(calculationId);
        result.Price.Should().Be(calculationModel.Price);
    }
    
    [Fact]
    public async Task Handle_ResultPriceIsMaxOfTwo()
    {
        //arrange
        var userId = Create.RandomId();
        var volume = Create.RandomDouble();
        var weight = Create.RandomDouble();
        var maxPrice = Create.RandomDecimal();
        
        var command = CalculateDeliveryPriceCommandFaker.Generate()
            .WithUserId(userId);
        
        var builder = new CalculateDeliveryPriceHandlerBuilder();
        builder.CalculationService
            .SetupCalculatePriceByWeight(weight, maxPrice)
            .SetupCalculatePriceByVolume(volume, maxPrice - 0.001m);

        var handler = builder.Build();
        
        //act
        var result = await handler.Handle(command, default);

        //asserts
        result.Price.Should().Be(maxPrice);
    }
    
    [Fact]
    public async Task Handle_ThrowsWhenNoGoods()
    {
        //arrange
        var command = CalculateDeliveryPriceCommandFaker.Generate()
            .WithGoods(Array.Empty<GoodModel>());
        
        var builder = new CalculateDeliveryPriceHandlerBuilder();
        var handler = builder.Build();
        
        //act
        var act = () => handler.Handle(command, default);

        //asserts
        await Assert.ThrowsAsync<GoodsNotFoundException>(act);
    }
    
}