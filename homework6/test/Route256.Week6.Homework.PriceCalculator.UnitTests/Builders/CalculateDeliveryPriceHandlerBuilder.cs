using Moq;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week5.Workshop.PriceCalculator.UnitTests.Stubs;

namespace Route256.Week5.Workshop.PriceCalculator.UnitTests.Builders;

public class CalculateDeliveryPriceHandlerBuilder
{
    public Mock<ICalculationService> CalculationService;
    
    public CalculateDeliveryPriceHandlerBuilder()
    {
        CalculationService = new Mock<ICalculationService>();
    }
    
    public CalculateDeliveryPriceCommandHandlerStub Build()
    {
        return new CalculateDeliveryPriceCommandHandlerStub(
            CalculationService);
    }
}