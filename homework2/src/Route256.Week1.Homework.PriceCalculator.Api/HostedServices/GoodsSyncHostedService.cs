using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Bll;
using Microsoft.Extensions.Options;

namespace Route256.Week1.Homework.PriceCalculator.Api.HostedServices;

public sealed class GoodsSyncHostedService: BackgroundService
{
    private readonly IGoodsRepository _repository;
    private readonly IServiceProvider _serviceProvider;
    private ILogger<GoodsSyncHostedService> _logger;

    public GoodsSyncHostedService(
        IGoodsRepository repository,
        IServiceProvider serviceProvider,
        ILogger<GoodsSyncHostedService> logger)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var goodsService = scope.ServiceProvider.GetRequiredService<IGoodsService>();
                var goods = goodsService.GetGoods().ToList();
                foreach (var good in goods)
                    _repository.AddOrUpdate(good);
                await Task.Delay(TimeSpan.FromSeconds(
                    scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<UpdateOptions>>()
                    .Value.FrequencyOfUpdates), 
                    stoppingToken);
            }
        }
    }
}