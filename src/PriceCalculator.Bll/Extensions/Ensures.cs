using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Exceptions;
using PriceCalculator.Bll.Queries;

namespace PriceCalculator.Bll.Extensions;

public static class Ensures
{
    public static CalculateDeliveryPriceCommand EnsureHasGoods(
        this CalculateDeliveryPriceCommand src)
    {
        if (!src.Goods.Any())
        {
            throw new GoodsNotFoundException("Товары не найдены");
        }

        return src;
    }
}