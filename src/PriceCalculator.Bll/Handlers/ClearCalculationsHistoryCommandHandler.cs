using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Exceptions;
using PriceCalculator.Bll.Extensions;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.Dal.Models;

namespace PriceCalculator.Bll.Handlers;

public class ClearCalculationsHistoryCommandHandler 
    : IRequestHandler<ClearCalculationsHistoryCommand>
{
    private readonly ICalculationService _calculationService;

    public ClearCalculationsHistoryCommandHandler(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public async Task Handle(ClearCalculationsHistoryCommand request, CancellationToken cancellationToken)
    {
        var queryModel = new QueryCalculationFilter(
            UserId: request.UserId,
            Limit: int.MaxValue,
            Offset: 0
        );

        var calculations = await _calculationService.QueryCalculations(queryModel, cancellationToken);
        var calculationIds = calculations.Select(x => x.Id).ToArray();

        if (!calculations.Any())
            throw new OneOrManyCalculationsNotFoundException("Расчеты не найдены");

        request.EnsureBelongsToUser(calculationIds);
        
        await _calculationService.RemoveCalculations(calculations, cancellationToken);
    }
}