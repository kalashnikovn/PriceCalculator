using MediatR;

namespace PriceCalculator.Bll.Commands;

public record SaveAnomalyCommand(
    long GoodId,
    decimal Price) : IRequest<long>;