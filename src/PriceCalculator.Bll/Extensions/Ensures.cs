using Microsoft.IdentityModel.Tokens;
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
    

    public static ClearCalculationsHistoryCommand EnsureBelongsToUser(
        this ClearCalculationsHistoryCommand src,
        long[] calculationIds
        )
    {

        var requestCalculationIdsSet = new HashSet<long>(src.CalculationIds);
        var calculationIdsSet = new HashSet<long>(calculationIds);
        var differences = requestCalculationIdsSet.Except(calculationIdsSet).ToArray();

        if (!differences.IsNullOrEmpty()) 
            throw new OneOrManyCalculationsBelongsToAnotherUserException(
                "One or many calculation IDs belong to another user.",
                differences);

        return src;
    }
}