using Moq;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.UnitTests.Stubs;

namespace Route256.Week5.Homework.PriceCalculator.UnitTests.Builders;

public class CalculationServiceBuilder
{
    public Mock<ICalculationRepository> CalculationRepository;
    public Mock<IGoodsRepository> GoodsRepository;
    
    public CalculationServiceBuilder()
    {
        CalculationRepository = new Mock<ICalculationRepository>();
        GoodsRepository = new Mock<IGoodsRepository>();
    }
    
    public CalculationServiceStub Build()
    {
        return new CalculationServiceStub(
            CalculationRepository,
            GoodsRepository);
    }
}