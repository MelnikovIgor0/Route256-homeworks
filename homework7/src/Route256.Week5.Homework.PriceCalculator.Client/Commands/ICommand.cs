namespace Route256.Week5.Homework.PriceCalculator.Client.Commands;

public interface ICommand
{
    public Task Execute(PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient client);
}