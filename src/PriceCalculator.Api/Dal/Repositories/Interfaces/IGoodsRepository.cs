using PriceCalculator.Api.Dal.Entities;

namespace PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodEntity entity);
    ICollection<GoodEntity> GetAll();
    GoodEntity Get(int id);

}