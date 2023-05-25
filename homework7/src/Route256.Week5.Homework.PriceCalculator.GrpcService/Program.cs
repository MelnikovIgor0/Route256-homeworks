using Route256.Week5.Homework.PriceCalculator.Bll.Extensions;
using Route256.Week5.Homework.PriceCalculator.Dal.Extensions;
using Route256.Week5.Homework.PriceCalculator.GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddGrpcReflection().AddGrpc();

services
    .AddBll()
    .AddDalInfrastructure(builder.Configuration)
    .AddDalRepositories();

var app = builder.Build();
app.MapGrpcReflectionService();
app.MapGrpcService<MyPriceCalculatorGrpcApi>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateUp();
app.Run();