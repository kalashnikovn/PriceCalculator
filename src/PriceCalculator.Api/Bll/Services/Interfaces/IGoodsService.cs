using PriceCalculator.Api.Dal.Entities;

namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}