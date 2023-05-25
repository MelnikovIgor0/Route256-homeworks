using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Services;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Route256.PriceCalculator.Domain.Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));
        services.AddScoped(x => x.GetRequiredService<IOptionsSnapshot<PriceCalculatorOptions>>().Value);
        return services;
    }
}
