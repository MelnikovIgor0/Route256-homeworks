using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.ActionFilters;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V2;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V2;
using Route256.Week1.Homework.PriceCalculator.Api.Validators;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v2/[controller]")]
public class V2DeliveryPriceController : Controller
{
    private readonly ILogger<V2DeliveryPriceController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V2DeliveryPriceController(
        ILogger<V2DeliveryPriceController> logger,
        IHttpContextAccessor httpContextAccessor,
        IPriceCalculatorService priceCalculatorService)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
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
        _httpContextAccessor.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        var sr = new StreamReader(_httpContextAccessor.HttpContext.Request.Body);
        var bodyString = await sr.ReadToEndAsync();
        _logger.LogInformation(bodyString);

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