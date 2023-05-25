namespace Route256.Week5.Homework.PriceCalculator.Bll.Models;

public record GetHistoryQueryResult(
    GetHistoryQueryResult.HistoryItem[] Items)
{
    public record HistoryItem(
        double Volume,
        double Weight,
        decimal Price,
        long[] GoodIds);
}