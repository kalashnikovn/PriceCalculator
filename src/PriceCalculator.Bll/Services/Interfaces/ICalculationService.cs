using PriceCalculator.Bll.Models;

namespace PriceCalculator.Bll.Services.Interfaces;

public interface ICalculationService
{
    decimal CalculatePriceByVolume(
        GoodModel[] goods,
        out double volume);

    public decimal CalculatePriceByWeight(
        GoodModel[] goods,
        out double weight);
    
    Task<long> SaveCalculation(
        SaveCalculationModel saveCalculation,
        CancellationToken cancellationToken);
    
    Task<QueryCalculationModel[]> QueryCalculations(
        QueryCalculationFilter query,
        CancellationToken token);

    Task<int> RemoveCalculations(
        RemoveCalculationsModel removeCalculations,
        CancellationToken cancellationToken);
}