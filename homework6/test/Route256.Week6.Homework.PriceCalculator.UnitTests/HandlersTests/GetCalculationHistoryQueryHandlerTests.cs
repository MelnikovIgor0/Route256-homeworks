using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Route256.Week5.Workshop.PriceCalculator.UnitTests.Builders;
using Route256.Week5.Workshop.PriceCalculator.UnitTests.Extensions;
using Route256.Week5.Workshop.PriceCalculator.UnitTests.Fakers;
using Route256.Week5.Workshop.TestingInfrastructure.Creators;
using Xunit;

namespace Route256.Week5.Workshop.PriceCalculator.UnitTests.HandlersTests;

public class GetCalculationHistoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_MakeAllCalls()
    {
        //arrange
        var userId = Create.RandomId();

        var command = GetCalculationHistoryQueryFaker.Generate()
            .WithUserId(userId);

        var queryModels = QueryCalculationModelFaker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var filter = QueryCalculationFilterFaker.Generate()
            .WithUserId(userId)
            .WithLimit(command.Take)
            .WithOffset(command.Skip);

        var builder = new GetCalculationHistoryHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculations(queryModels);

        var handler = builder.Build();

        //act
        var result = await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifyQueryCalculationsWasCalledOnce(filter);

        handler.VerifyNoOtherCalls();

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(queryModels.Length);
        result.Items.Select(x => x.Price)
            .Should().IntersectWith(queryModels.Select(x => x.Price));
        result.Items.Select(x => x.Volume)
            .Should().IntersectWith(queryModels.Select(x => x.TotalVolume));
        result.Items.Select(x => x.Weight)
            .Should().IntersectWith(queryModels.Select(x => x.TotalWeight));
    }

}