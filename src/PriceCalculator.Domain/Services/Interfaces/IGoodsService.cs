using PriceCalculator.Domain.Entities;

namespace PriceCalculator.Domain.Services.Interfaces;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}