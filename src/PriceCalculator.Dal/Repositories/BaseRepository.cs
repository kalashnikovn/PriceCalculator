using System.Transactions;
using Npgsql;
using PriceCalculator.Dal.Options;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Dal.Repositories;

public abstract class BaseRepository : IDbRepository
{
    private readonly DalOptions _dalOptions;

    protected BaseRepository(DalOptions dalOptions)
    {
        _dalOptions = dalOptions;
    }

    protected async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection(_dalOptions.ConnectionString);
        await connection.OpenAsync();
        
        // обновление типов для разных подключений
        connection.ReloadTypes();
        
        return connection;
    }
    
    public TransactionScope CreateTransactionScope(IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions 
            { 
                IsolationLevel = level, 
                Timeout = TimeSpan.FromSeconds(5) 
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}