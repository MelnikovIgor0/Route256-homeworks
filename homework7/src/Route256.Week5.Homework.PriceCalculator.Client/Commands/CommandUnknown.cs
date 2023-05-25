namespace Route256.Week5.Homework.PriceCalculator.Client.Commands;

public class CommandUnknown : ICommand
{
    public async Task Execute(PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient client)
    {
        Console.WriteLine("Unknown command");
    }
}