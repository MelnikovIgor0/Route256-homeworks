namespace Route256.Domain.Models.PriceCalculator;

public class GoodModel
{
    public int Id;
    public int Height;
    public int Length;
    public int Width;
    public int Weight;

    public GoodModel(int id, int height, int length, int width, int weight)
    {
        Id = id;
        Height = height;
        Length = length;
        Width = width;
        Weight = weight;
    }

    public static GoodModel Parse(string line)
    {
        const int INDEX_ID = 0;
        const int INDEX_HEIGHT = 1;
        const int INDEX_LENGTH = 2;
        const int INDEX_WIDTH = 3;
        const int INDEX_WEIGHT = 4;
        string[] values = line.Split(',');
        return new GoodModel(
            int.Parse(values[INDEX_ID].Replace(" ", string.Empty)),
            int.Parse(values[INDEX_HEIGHT].Replace(" ", string.Empty)),
            int.Parse(values[INDEX_LENGTH].Replace(" ", string.Empty)),
            int.Parse(values[INDEX_WIDTH].Replace(" ", string.Empty)),
            int.Parse(values[INDEX_WEIGHT].Replace(" ", string.Empty)));
    }
}
