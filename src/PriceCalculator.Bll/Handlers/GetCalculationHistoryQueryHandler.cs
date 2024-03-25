using MediatR;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Queries;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.Bll.Handlers;

public class GetCalculationHistoryQueryHandler
    : IRequestHandler<GetCalculationHistoryQuery, GetHistoryQueryResult>
{
    private readonly ICalculationService _calculationService;

    public GetCalculationHistoryQueryHandler(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public async Task<GetHistoryQueryResult> Handle(
        GetCalculationHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var queryFilter = new QueryCalculationFilter(
            UserId: request.UserId,
            Limit: request.Take,
            Offset: request.Skip);
        
        
        var logs = await _calculationService.QueryCalculations(queryFilter, cancellationToken);

        var items = logs.Select(x => 
                new GetHistoryQueryResult.HistoryItem(
                    x.TotalVolume,
                    x.TotalWeight,
                    x.Price,
                    x.GoodIds))
            .ToArray();

        return new GetHistoryQueryResult(items);
    }
}