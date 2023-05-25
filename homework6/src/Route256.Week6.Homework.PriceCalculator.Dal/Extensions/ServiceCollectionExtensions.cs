using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Route256.Week5.Workshop.PriceCalculator.Dal.Infrastructure;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Workshop.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICalculationRepository, CalculationRepository>();
        services.AddScoped<IGoodsRepository, GoodsRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddDalInfrastructure(
        this IServiceCollection services, 
        IConfigurationRoot config)
    {
        //read config
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));

        //configure postrges types
        Postgres.MapCompositeTypes();
        
        //add migrations
        Postgres.AddMigrations(services);
        
        return services;

    }
}