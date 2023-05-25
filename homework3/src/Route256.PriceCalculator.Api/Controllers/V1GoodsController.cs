using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Domain.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Api.Responses.V2;

namespace Route256.PriceCalculator.Api.Controllers;

[Route("goods")]
[ApiController]
public sealed class V1GoodsController
{
    private readonly IGoodsRepository _repository;

    public V1GoodsController(
        IGoodsRepository repository)
    {
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
        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);
        
        var price = priceCalculatorService.CalculatePrice(new []{ model });
        return new CalculateResponse(price);
    }
}