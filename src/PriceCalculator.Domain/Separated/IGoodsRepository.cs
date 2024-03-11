using PriceCalculator.Domain.Entities;

namespace PriceCalculator.Domain.Separated;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodEntity entity);
    ICollection<GoodEntity> GetAll();
    GoodEntity Get(int id);

}