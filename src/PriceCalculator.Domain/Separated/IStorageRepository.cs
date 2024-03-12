using PriceCalculator.Domain.Entities;

namespace PriceCalculator.Domain.Separated;

public interface IStorageRepository
{
    void Save(StorageEntity entity);
    IReadOnlyList<StorageEntity> Query();
    void Clear();
}