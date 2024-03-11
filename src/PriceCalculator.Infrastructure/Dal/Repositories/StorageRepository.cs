
using PriceCalculator.Domain.Entities;
using PriceCalculator.Domain.Separated;

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

    public void Clear() => _storage.Clear();
}