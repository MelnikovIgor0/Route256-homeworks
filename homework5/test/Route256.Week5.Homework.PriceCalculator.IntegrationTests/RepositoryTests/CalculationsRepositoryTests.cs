using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.IntegrationTests.Fixtures;
using Route256.Week5.Homework.TestingInfrastructure.Creators;
using Route256.Week5.Homework.TestingInfrastructure.Fakers;
using Xunit;

namespace Route256.Week5.Homework.PriceCalculator.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class CalculationsRepositoryTests
{
    private readonly double _requiredDoublePrecision = 0.00001d;
    private readonly decimal _requiredDecimalPrecision = 0.00001m;
    private readonly TimeSpan _requiredDateTimePrecision = TimeSpan.FromMilliseconds(100);
    
    private readonly ICalculationRepository _calculationRepository;

    public CalculationsRepositoryTests(TestFixture fixture)
    {
        _calculationRepository = fixture.CalculationRepository;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Add_Calculations_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        
        // Act
        var calculationIds = await _calculationRepository.Add(calculations, default);

        // Asserts
        calculationIds.Should().HaveCount(count);
        calculationIds.Should().OnlyContain(x => x > 0);
    }
    
    [Fact]
    public async Task Query_SingleCalculation_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate()
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        var expected = calculations.Single();
        
        var calculationId = (await _calculationRepository.Add(calculations, default))
            .Single();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(userId, 1, 0), 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(1);
        var calculation = foundCalculations.Single();

        calculation.Id.Should().Be(calculationId);
        calculation.UserId.Should().Be(expected.UserId);
        calculation.At.Should().BeCloseTo(expected.At, _requiredDateTimePrecision);
        calculation.Price.Should().BeApproximately(expected.Price, _requiredDecimalPrecision);
        calculation.GoodIds.Should().BeEquivalentTo(expected.GoodIds);
        calculation.TotalVolume.Should().BeApproximately(expected.TotalVolume, _requiredDoublePrecision);
        calculation.TotalWeight.Should().BeApproximately(expected.TotalWeight, _requiredDoublePrecision);
    }
    
    [Theory]
    [InlineData(3,  2, 3)]
    [InlineData(1,  6, 1)]
    [InlineData(2,  8, 2)]
    [InlineData(3, 10, 0)]
    public async Task Query_CalculationsInRange_Success(int take, int skip, int expectedCount)
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        await _calculationRepository.Add(calculations, default);

        var allCalculations = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(userId, 100, 0), 
            default);
        
        var expected = allCalculations
            .OrderByDescending(x => x.At)
            .Skip(skip)
            .Take(take);
        
        // Act
        var foundCalculations = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(userId, take, skip), 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(expectedCount);

        if (expectedCount > 0)
        {
            foundCalculations.Should().BeEquivalentTo(expected);
        }
    }
    
    [Fact]
    public async Task Query_AllCalculations_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(userId, 100, 0), 
            default);

        // Assert
        foundCalculations.Should().NotBeEmpty();
        foundCalculations.Should().OnlyContain(x => x.UserId == userId);
        foundCalculations.Should().OnlyContain(x => calculationIds.Contains(x.Id));
        foundCalculations.Should().BeInDescendingOrder(x => x.At);
    }
    
    [Fact]
    public async Task Query_Calculations_ReturnsEmpty_WhenForWrongUser()
    {
        // Arrange
        var userId = Create.RandomId();
        var anotherUserId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new CalculationHistoryQueryModel(anotherUserId, 100, 0), 
            default);

        // Assert
        foundCalculations.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Query_AreCalculationIdsInvalid_ReturnsTrueIds_WhenWrongIds()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        long badId = calculations.Select(x => x.Id).Max() + 1;
        bool ans = await _calculationRepository.AreCalculationIdsInvalid(
            new ClearHistoryQueryModel(userId, new long[] { badId }),
            default);
        ans.Should().BeTrue();
    }
    
    [Fact]
    public async Task Query_AreCalculationIdsInvalid_ReturnsFalse_WhenNoWrongIds()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        bool ans = await _calculationRepository.AreCalculationIdsInvalid(
            new ClearHistoryQueryModel(userId, new long[] {calculationIds.Max()}),
            default);
        ans.Should().BeFalse();
    }
    
    [Fact]
    public async Task Query_AreCalculationIdsInvalid_ReturnsFalse_WhenArrayOfIdsEmpty()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        bool ans = await _calculationRepository.AreCalculationIdsInvalid(
            new ClearHistoryQueryModel(userId, new long[]{}),
            default);
        ans.Should().BeFalse();
    }
    
    [Fact]
    public async Task Query_GetInvalidIdsByUser_EmptyArrayOfInvalidIds_WhenAllIdsAreValid()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        var ans = await _calculationRepository.GetInvalidIdsByUser(
            new ClearHistoryQueryModel(userId, calculationIds.ToArray()), default);
        ans.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Query_GetInvalidIdsByUser_CorrectArrayOfInvalidIds_WhenAllIdsAreInvalid()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        var ans = await _calculationRepository.GetInvalidIdsByUser(
            new ClearHistoryQueryModel(userId + 1, calculationIds.ToArray()), default);
        ans.Should().BeEquivalentTo(calculationIds);
    }
    
    [Fact]
    public async Task Query_GetInvalidIdsByUser_EmptyArrayOfInvalidIds_WhenArrayOfIdsEmpty()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        var ans = await _calculationRepository.GetInvalidIdsByUser(
            new ClearHistoryQueryModel(userId, new long[]{}), default);
        ans.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Query_ClearHistory_WithUsingQueryWithEmptyArray_RepositoryClear()
    {
        // Arrange
        long userId = 1;
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();
        await _calculationRepository.ClearHistoryByEmptyList(
            new ClearHistoryQueryModel(userId, new long[] { }), default);
        _calculationRepository.Query(
            new CalculationHistoryQueryModel(userId, 100, 0), default)
            .Result.Should().BeEmpty();
    }
}
