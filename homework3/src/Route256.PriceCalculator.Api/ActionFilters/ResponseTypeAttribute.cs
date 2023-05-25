using Microsoft.AspNetCore.Mvc;

namespace Route256.PriceCalculator.Api.ActionFilters;

internal sealed class ResponseTypeAttribute: ProducesResponseTypeAttribute
{
    public ResponseTypeAttribute(int statusCode) 
        : base(typeof(ErrorResponse), statusCode)
    {
    }
}