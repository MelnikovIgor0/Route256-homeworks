using Moq;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Stubs;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Builders;

public class GetCalculationHistoryHandlerBuilder
{
    public Mock<ICalculationService> CalculationService;
    
    public GetCalculationHistoryHandlerBuilder()
    {
        CalculationService = new Mock<ICalculationService>();
    }
    
    public GetCalculationHistoryHandlerStub Build()
    {
        return new GetCalculationHistoryHandlerStub(
            CalculationService);
    }
}