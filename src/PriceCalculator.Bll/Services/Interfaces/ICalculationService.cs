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
    
    Task<QueryCalculationModel[]> QueryCalculations(
        long[] calculationIds,
        CancellationToken token);

    Task RemoveCalculations(
        QueryCalculationModel[] removeCalculationsModel,
        CancellationToken cancellationToken);
    
    Task RemoveCalculations(
        long userId,
        CancellationToken cancellationToken);
}