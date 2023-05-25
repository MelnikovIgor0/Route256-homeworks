// using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;
// using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
// using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories;
// using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// var services = builder.Services;
// services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// services.AddEndpointsApiExplorer();
// services.AddSwaggerGen(o =>
// {
//     o.CustomSchemaIds(x => x.FullName);
// });
// services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
// services.AddSingleton<IStorageRepository, StorageRepository>();
//
//
// var app = builder.Build();
//
//
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.MapControllers();
//
// app.Run();
