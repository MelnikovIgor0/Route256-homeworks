namespace Route256.Week5.Homework.PriceCalculator.Api.Responses.V1;

public record GetHistoryResponse(
    CargoResponse Cargo,
    decimal Price);
    