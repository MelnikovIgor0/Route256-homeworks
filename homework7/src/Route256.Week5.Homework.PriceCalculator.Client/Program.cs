using Grpc.Core;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using PriceCalculatorGrpcApi = Route256.Week5.Homework.PriceCalculator.Client.PriceCalculatorGrpcApi;
using Route256.Week5.Homework.PriceCalculator.Client;
using System.IO;
using Route256.Week5.Homework.PriceCalculator.Client.Commands;

using var channel = GrpcChannel.ForAddress("http://localhost:5030");
var client = new PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient(channel);

while (true)
{
    string command = Console.ReadLine();
    if (command == "exit")
    {
        break;
    }
    ICommand currentCommand;
    switch (command)
    {
        case "calculate":
            currentCommand = new CommandCalculate();
            break;
        case "gethistory":
            currentCommand = new CommandCalculate();
            break;
        case "clearhistory":
            currentCommand = new CommandClearHistory();
            break;
        default:
            currentCommand = new CommandUnknown();
            break;
    }
    await currentCommand.Execute(client);
}
