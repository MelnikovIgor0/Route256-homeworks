namespace Route256.Week5.Workshop.PriceCalculator.Api.Requests.V1;

public record GetHistoryRequest(
    long UserId,
    int Take,
    int Skip);