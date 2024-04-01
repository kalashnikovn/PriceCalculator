using PriceCalculator.Dal.Entities;

namespace PriceCalculator.Dal.Repositories.Interfaces;

public interface IAnomalyRepository : IDbRepository
{
    Task<long[]> Add(
        AnomalyEntityV1[] entities,
        CancellationToken cancellationToken);
}