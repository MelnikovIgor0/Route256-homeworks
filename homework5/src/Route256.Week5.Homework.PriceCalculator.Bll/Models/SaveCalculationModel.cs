namespace Route256.Week5.Homework.PriceCalculator.Bll.Models;

public record SaveCalculationModel(
    long UserId, 
    double TotalVolume, 
    double TotalWeight, 
    decimal Price, 
    GoodModel[] Goods);