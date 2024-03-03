using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const double _volumeRatio = 3.27d;
    
    public double CalculatePrice(GoodModel[] goods)
    {
        var volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        var volumePrice = volume * _volumeRatio;

        return volumePrice;

    }

    public CalculationLogModel QueryLog(int take)
    {
        
        throw new NotImplementedException();
    }
}