using PriceCalculator.Api.Bll.Models;

namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    double CalculatePrice(GoodModel[] goods);
    CalculationLogModel QueryLog(int take);
}