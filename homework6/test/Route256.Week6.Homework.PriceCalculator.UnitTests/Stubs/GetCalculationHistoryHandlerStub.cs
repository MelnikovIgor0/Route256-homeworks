using Moq;
using Route256.Week5.Workshop.PriceCalculator.Bll.Queries;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Workshop.PriceCalculator.UnitTests.Stubs;

public class GetCalculationHistoryHandlerStub : GetCalculationHistoryQueryHandler
{
    public Mock<ICalculationService> CalculationService { get; }
    
    public GetCalculationHistoryHandlerStub(
        Mock<ICalculationService> calculationService) 
        : base(
            calculationService.Object)
    {
        CalculationService = calculationService;
    }
    
    public void VerifyNoOtherCalls()
    {
        CalculationService.VerifyNoOtherCalls();
    }
}