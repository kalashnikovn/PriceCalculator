using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Bll.Services;

public sealed class AnomalyService : IAnomalyService
{
    private readonly IAnomalyRepository _anomalyRepository;

    public AnomalyService(
        IAnomalyRepository anomalyRepository)
    {
        _anomalyRepository = anomalyRepository;
    }
    
    public async Task<long> SaveAnomaly(SaveAnomalyModel anomalyModel, CancellationToken cancellationToken)
    {
        var anomalyEntity = new AnomalyEntityV1()
        {
            GoodId = anomalyModel.GoodId,
            Price = anomalyModel.Price
        };

        var ids = await _anomalyRepository.Add(
            new[] { anomalyEntity },
            cancellationToken);

        return ids.Single();
    }
}