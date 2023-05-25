var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(o =>
{
    o.CustomSchemaIds(x => x.FullName);
});
services.AddScoped<Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V1.Interfaces.IPriceCalculatorService, 
    Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V1.PriceCalculatorService>();
services.AddSingleton<Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces.IStorageRepository<Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V1.StorageEntity>,
    Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.V1.StorageRepository>();

services.AddScoped<Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V3.Interfaces.IPriceCalculatorService, 
    Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.V3.PriceCalculatorService>();
services.AddSingleton<Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces.IStorageRepository<Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities.V3.StorageEntity>, 
    Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.V3.StorageRepository>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
