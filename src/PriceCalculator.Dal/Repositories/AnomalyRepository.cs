using Dapper;
using Microsoft.Extensions.Options;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Options;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Dal.Repositories;

public sealed class AnomalyRepository : BaseRepository, IAnomalyRepository
{
    public AnomalyRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<long[]> Add(AnomalyEntityV1[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
insert into anomalies (good_id, price)
select good_id, price 
from UNNEST(@Anomalies)
returning id
";
        var sqlQueryParams = new
        {
            Anomalies = entities
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
}