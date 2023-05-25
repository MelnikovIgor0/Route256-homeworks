namespace Route256.Week5.Workshop.PriceCalculator.Bll.Models;

public record QueryCalculationModel(
    long Id,
    long UserId, 
    double TotalVolume, 
    double TotalWeight, 
    decimal Price, 
    long[] GoodIds);