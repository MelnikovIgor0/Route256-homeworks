using System.Net;
using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.ActionFilters;
using Route256.PriceCalculator.Domain;
using Route256.PriceCalculator.Domain.Services;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Route256.PriceCalculator.Infrastructure.Dal.Repositories;
using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Api.HostedServices;
using Microsoft.Extensions.Options;

namespace Route256.PriceCalculator.Api;

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
        services.AddDomain(_configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(x => x.FullName);
        });
        services.AddServices();
        services.AddInfrastructure();
        services.AddHostedService<GoodsSyncHostedService>();
        services.AddHttpContextAccessor();
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}