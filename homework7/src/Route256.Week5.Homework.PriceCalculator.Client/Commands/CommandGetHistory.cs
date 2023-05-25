using Grpc.Core;

namespace Route256.Week5.Homework.PriceCalculator.Client.Commands;

public class CommandGetHistory : ICommand
{
    public async Task Execute(PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient client)
    {
        string[] data = Console.ReadLine().Split(' ');
        await foreach (HistoryResponse element in client.History(new HistoryRequest()
                       {
                           UserId = long.Parse(data[0]),
                           Take = long.Parse(data[1]),
                           Skip = long.Parse(data[2])
                       }).ResponseStream.ReadAllAsync())
        {
            Console.WriteLine(element);
        }
    }
}