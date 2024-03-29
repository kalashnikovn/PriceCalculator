﻿using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Models;

namespace PriceCalculator.Dal.Repositories.Interfaces;

public interface ICalculationsRepository : IDbRepository
{
    Task<long[]> Add(CalculationEntityV1[] entitiesV1, CancellationToken cancellationToken);

    Task<CalculationEntityV1[]> Query(GetHistoryQueryModel query, CancellationToken cancellationToken);
    Task<CalculationEntityV1[]> Query(
        long[] calculationIds,
        CancellationToken cancellationToken);

    Task<int> Remove(long[] ids, CancellationToken cancellationToken);
    Task<int> Remove(long userId, CancellationToken cancellationToken);
}