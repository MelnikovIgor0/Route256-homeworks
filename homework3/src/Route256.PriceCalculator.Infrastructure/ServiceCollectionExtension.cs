using Route256.PriceCalculator.Domain.Separated;
using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Route256.PriceCalculator.Domain.Services;

namespace Route256.PriceCalculator.Infrastructure.Dal.Repositories;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddScoped<IGoodPriceCalculatorService, GoodPriceCalculatorService>();
        services.AddScoped<IGoodsService, GoodsService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();
        return services;
    }
}
