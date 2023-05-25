namespace Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

public record GetHistoryResponse(
    CargoResponse Cargo,
    decimal Price);