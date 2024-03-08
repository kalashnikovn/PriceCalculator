namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IGoodsFullPriceService
{
    decimal GetFullPrice(int id);
}