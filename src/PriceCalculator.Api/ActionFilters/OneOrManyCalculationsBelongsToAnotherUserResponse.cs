namespace PriceCalculator.Api.ActionFilters;

public record OneOrManyCalculationsBelongsToAnotherUserResponse(
    long[] WrongCalculationIds);