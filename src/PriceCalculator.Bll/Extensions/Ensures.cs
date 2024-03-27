using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Exceptions;
using PriceCalculator.Bll.Models;

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

    public static ClearCalculationsHistoryCommand EnsureAllFound(
        this ClearCalculationsHistoryCommand src,
        QueryCalculationModel[] calculations)
    {
        if (src.CalculationIds.Length > calculations.Length)
            throw new OneOrManyCalculationsNotFoundException();
        
        return src;
    }
    

    public static ClearCalculationsHistoryCommand EnsureBelongsToOneUser(
        this ClearCalculationsHistoryCommand src,
        QueryCalculationModel[] calculations)
    {
        var wrongCalculationIds = calculations
            .Where(x => x.UserId != src.UserId)
            .Select(x => x.Id)
            .ToArray();

        if (wrongCalculationIds.Length != 0)
        {
            throw new OneOrManyCalculationsBelongsToAnotherUserException(
                "One or many calculation IDs belong to another user.",
                wrongCalculationIds);
        }
        
        return src;
    }
}