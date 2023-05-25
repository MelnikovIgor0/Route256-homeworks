namespace Route256.Week5.Workshop.PriceCalculator.Bll.Models;

public record SaveCalculationModel(
    long UserId, 
    double TotalVolume, 
    double TotalWeight, 
    decimal Price, 
    GoodModel[] Goods);