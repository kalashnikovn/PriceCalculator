using MediatR;

namespace PriceCalculator.Bll.Commands;

public record ClearCalculationsHistoryCommand(
    long UserId,
    long[] CalculationIds) : IRequest;