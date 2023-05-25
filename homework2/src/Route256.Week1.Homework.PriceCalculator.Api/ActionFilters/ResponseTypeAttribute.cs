using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Route256.Week1.Homework.PriceCalculator.Api.ActionFilters;

internal sealed class ResponseTypeAttribute: ProducesResponseTypeAttribute
{
    public ResponseTypeAttribute(int statusCode) 
        : base(typeof(ErrorResponse), statusCode)
    {
    }
}