using Dapper;
using Microsoft.Extensions.Options;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Models;
using PriceCalculator.Dal.Options;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Dal.Repositories;

public sealed class CalculationsRepository : BaseRepository, ICalculationsRepository
{
    public CalculationsRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<long[]> Add(CalculationEntityV1[] entitiesV1, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
insert into calculations (user_id, goods_id, total_volume, total_weight, price, at)
select user_id, goods_id, total_volume, total_weight, price, at
from UNNEST(@Calculations)
returning id;
";
        var sqlQueryParams = new
        {
            Calculations = entitiesV1
        };

        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken));

        return ids
            .ToArray();
    }

    public async Task<CalculationEntityV1[]> Query(GetHistoryQueryModel query, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
select id, user_id, goods_id, total_volume, total_weight, price, at
from calculations
where user_id = @UserId
order by at desc
limit @Limit offset @Offset
";

        var sqlQueryParams = new
        {
            UserId = query.UserId,
            Limit = query.Limit,
            Offset = query.Offset
        };

        await using var connection = await GetAndOpenConnection();
        var entities = await connection.QueryAsync<CalculationEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken));

        return entities
            .ToArray();
    }
}