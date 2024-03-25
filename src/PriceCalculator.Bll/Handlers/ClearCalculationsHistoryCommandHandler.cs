﻿using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Extensions;
using PriceCalculator.Bll.Services.Interfaces;

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
        var calculations = await _calculationService.QueryCalculations(
            request.CalculationIds,
            cancellationToken);

        request.EnsureAllFound(calculations);
        request.EnsureBelongsToOneUser(calculations);
        
        await _calculationService.RemoveCalculations(calculations, cancellationToken);
    }
}