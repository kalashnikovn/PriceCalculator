using PriceCalculator.Bll.Models;

namespace PriceCalculator.Bll.Services.Interfaces;

public interface IAnomalyService
{
    Task<long> SaveAnomaly(
        SaveAnomalyModel anomalyModel,
        CancellationToken cancellationToken);
}