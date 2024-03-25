using PriceCalculator.Dal.Entities;

namespace PriceCalculator.Dal.Repositories.Interfaces;

public interface IGoodsRepository : IDbRepository
{
    Task<long[]> Add(GoodEntityV1[] entities, CancellationToken cancellationToken);

    Task<GoodEntityV1[]> Query(long userId, CancellationToken cancellationToken);

    Task<int> Remove(long[] ids, CancellationToken cancellationToken);
}