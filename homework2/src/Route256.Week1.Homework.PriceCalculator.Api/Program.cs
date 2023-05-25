namespace Route256.Week1.Homework.PriceCalculator.Api;

internal sealed class Program
{
    public static void Main()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
        
        builder.Build().Run();
    }
}