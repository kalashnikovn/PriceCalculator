using PriceCalculator.Api.Dal.Entities;

namespace PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface IStorageRepository
{
    void Save(StorageEntity entity);
    IReadOnlyList<StorageEntity> Query();
    void Clear();
}