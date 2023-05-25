using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Domain.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Route256.PriceCalculator.Api.Responses.V2;
using Route256.PriceCalculator.Api.Validators;
using CalculateRequest = Route256.PriceCalculator.Api.Requests.V2.CalculateRequest;

namespace Route256.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v2/[controller]")]
public class V2DeliveryPriceController : Controller
{
    private readonly ILogger<V2DeliveryPriceController> _logger;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V2DeliveryPriceController(
        ILogger<V2DeliveryPriceController> logger,
        IHttpContextAccessor httpContextAccessor,
        IPriceCalculatorService priceCalculatorService)
    {
        _logger = logger;
        _priceCalculatorService = priceCalculatorService;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(CalculateRequest request)
    {
        var validator = new CalculateRequestValidator();
        await validator.ValidateAndThrowAsync(request);

        var price = _priceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight))
                .ToArray());
        
        return new CalculateResponse(price);
    }
}