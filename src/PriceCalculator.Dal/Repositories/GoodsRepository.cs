using Dapper;
using Microsoft.Extensions.Options;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Options;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Dal.Repositories;

public sealed class GoodsRepository : BaseRepository, IGoodsRepository
{
    public GoodsRepository(IOptions<DalOptions> dalOptions) : base(dalOptions.Value)
    {
    }

    public async Task<long[]> Add(GoodEntityV1[] entities, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
insert into goods (user_id, width, height, length, weight) 
select user_id, width, height, length, weight
  from UNNEST(@Goods)
returning id;
";
        var sqlQueryParams = new
        {
            Goods = entities
        };

        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken
                ));

        return ids
            .ToArray();
    }

    public async Task<GoodEntityV1[]> Query(long userId, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
select id
     , user_id
     , width
     , height
     , length
     , weight
from goods
where user_id = @UserId;
";
        var sqlQueryParams = new
        {
            UserId = userId
        };
        
        await using var connection = await GetAndOpenConnection();
        var entities = await connection.QueryAsync<GoodEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken
                ));

        return entities
            .ToArray();
    }

    public async Task<int> Remove(long[] ids, CancellationToken cancellationToken)
    {
        const string sqlCommand = @"
delete from goods 
where id = ANY(@Ids)
";

        var sqlQueryParams = new
        {
            Ids = ids
        };

        await using var connection = await GetAndOpenConnection();
        var rows = await connection.ExecuteAsync(new CommandDefinition(
            sqlCommand,
            sqlQueryParams,
            cancellationToken: cancellationToken
            ));

        return rows;
    }

    public async Task<int> Remove(long userId, CancellationToken cancellationToken)
    {
        const string sqlCommand = @"
delete from goods 
where user_id = @UserId
";

        var sqlQueryParams = new
        {
            UserId = userId
        };

        await using var connection = await GetAndOpenConnection();
        var rows = await connection.ExecuteAsync(new CommandDefinition(
            sqlCommand,
            sqlQueryParams,
            cancellationToken: cancellationToken
        ));

        return rows;
    }
}