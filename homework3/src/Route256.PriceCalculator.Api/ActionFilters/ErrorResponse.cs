using System.Net;

namespace Route256.PriceCalculator.Api.ActionFilters;

public class ErrorResponse
{
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }

    public ErrorResponse(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}