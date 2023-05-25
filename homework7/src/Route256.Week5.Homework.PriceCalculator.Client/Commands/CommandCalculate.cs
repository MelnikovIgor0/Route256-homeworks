namespace Route256.Week5.Homework.PriceCalculator.Client.Commands;

public class CommandCalculate : ICommand
{
    public async Task Execute(PriceCalculatorGrpcApi.PriceCalculatorGrpcApiClient client)
    {
        string[] data = File.ReadAllLines(Console.ReadLine());
        var request = new CalculatePriceRequest()
        {
            UserId = long.Parse(data[0]),
            Goods = { }
        };
        for (int i = 1; i < data.Length; ++i)
        {
            string[] line = data[i].Split(' ');
            request.Goods.Add(new GoodModel()
            {
                Height = double.Parse(line[0]),
                Length = double.Parse(line[1]),
                Width = double.Parse(line[2]),
                Weight = double.Parse(line[3])
            });
        }
        CalculatePriceResponse response = client.CalculatePrice(request);
        Console.WriteLine(response);
    }
}