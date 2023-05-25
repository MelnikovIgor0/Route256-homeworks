namespace Route256.Week1.Homework.PriceCalculator.Api.ViewModels;


public class GoodsPageViewModel
{
    public ICollection<GoodViewModel> Goods { get; set; }
}


public sealed record GoodViewModel(
    string Name,
    int Id,
    int Height,
    int Length,
    int Width,
    int Weight,
    int Count,
    decimal Price
);

