using MediatR;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.Bll.Queries;

public record GetCalculationHistoryQuery(
    long UserId,
    int Take,
    int Skip) : IRequest<GetHistoryQueryResult>;