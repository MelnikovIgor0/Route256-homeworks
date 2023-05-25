using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V2;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;

[Route("goods")]
[ApiController]
public sealed class V1GoodsController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<V1GoodsController> _logger;
    private readonly IGoodsRepository _repository;

    public V1GoodsController(
        IHttpContextAccessor httpContextAccessor,
        ILogger<V1GoodsController> logger,
        IGoodsRepository repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _repository = repository;
    }
    
    [HttpGet]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("calculate/{id}")]
    public CalculateResponse Calculate(
        [FromServices] IPriceCalculatorService priceCalculatorService,
        int id)
    {
        _logger.LogInformation(_httpContextAccessor.HttpContext?.Request.Path);
        
        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);
        
        var price = priceCalculatorService.CalculatePrice(new []{ model });
        return new CalculateResponse(price);
    }

    [HttpPost("calculate-total-price")]
    public CalculateTotalPriceResponse CalculateTotalPrice(
        [FromServices] ITotalPriceCalculatorService totalPriceCalculatorService, int id)
    {
        _logger.LogInformation(_httpContextAccessor.HttpContext?.Request.Path);

        var good = _repository.Get(id);

        return new CalculateTotalPriceResponse(totalPriceCalculatorService.CalculateTotalPrice(good));
    }
}