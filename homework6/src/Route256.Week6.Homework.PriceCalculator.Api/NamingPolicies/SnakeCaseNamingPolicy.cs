using System.Text.Json;
using Route256.Week5.Workshop.PriceCalculator.Api.Extensions;

namespace Route256.Week5.Workshop.PriceCalculator.Api.NamingPolicies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
            name.ToSnakeCase();
}