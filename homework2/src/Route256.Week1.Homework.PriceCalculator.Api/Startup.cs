using System.Net;
using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.ActionFilters;
using Route256.Week1.Homework.PriceCalculator.Api.Bll;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.HostedServices;
using Route256.Week1.Homework.PriceCalculator.Api.Middlewaries;

namespace Route256.Week1.Homework.PriceCalculator.Api;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .AddMvcOptions(x =>
            {
                x.Filters.Add(new ExceptionFilterAttribute());
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
                x.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
            });
        
        services.Configure<PriceCalculatorOptions>(_configuration.GetSection("PriceCalculatorOptions"));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(x => x.FullName);
        });
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddScoped<ITotalPriceCalculatorService, TotalPriceCalculatorService>();
        services.AddHostedService<GoodsSyncHostedService>();
        services.Configure<UpdateOptions>(_configuration.GetSection("UpdateOptions"));
        
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();
        services.AddScoped<IGoodsService, GoodsService>();
        services.AddHttpContextAccessor();
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next.Invoke();
        });

        app.UseMiddleware<ErrorMiddleware>();
        app.UseMiddleware<LogRequestOfTotalPriceOfGoodMiddleware>();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllerRoute("goods-page", "goods-page", new
            {
                Controller = "GoodsView",
                Action = "Index"
            });
        });
    }
}