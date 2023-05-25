namespace Route256.Week5.Homework.PriceCalculator.Api.Requests.V2;

public record GetHistoryV2Request(
    long? UserId,
    long[]? CalculationIds,
    int Take,
    int Skip
);