using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Dal.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly List<StorageEntity> _storage = new();
    
    public void Save(StorageEntity entity)
    {
        _storage.Add(entity);
    }

    public IReadOnlyList<StorageEntity> Query()
    {
        return _storage.ToArray();
    }
}