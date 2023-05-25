namespace Route256.Domain.Entities;

public sealed class GoodEntity {
    public string Name;
    public int Id;
    public int Height;
    public int Length;
    public int Width;
    public int Weight;
    public int Count;
    public decimal Price;

    public GoodEntity(string name, int id, int height, int length, int width, int weight, int count, decimal price)
    {
        Name = name;
        Id = id;
        Height = height;
        Length = length;
        Width = width;
        Weight = weight;
        Count = count;
        Price = price;
    }
}