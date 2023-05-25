namespace Route256.Week5.Homework.PriceCalculator.Client.Commands;

public class CommandClearHistory : ICommand
{
    public async Task Execute(PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient client)
    {
        string[] data = Console.ReadLine().Split(' ');
        ClearHistoryRequest request = new ClearHistoryRequest()
        {
            UserId = long.Parse(data[0]),
            CalculationIds = { }
        };
        for (int i = 1; i < data.Length; ++i)
        {
            request.CalculationIds.Add(long.Parse(data[i]));
        }
        client.ClearHistory(request, options: default);
        Console.WriteLine("success");
    }
}