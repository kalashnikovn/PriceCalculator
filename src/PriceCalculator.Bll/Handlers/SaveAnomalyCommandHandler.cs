using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.Bll.Handlers;

public sealed class SaveAnomalyCommandHandler
    : IRequestHandler<SaveAnomalyCommand, long>
{
    private readonly IAnomalyService _anomalyService;

    public SaveAnomalyCommandHandler(
        IAnomalyService anomalyService)
    {
        _anomalyService = anomalyService;
    }
    
    public async Task<long> Handle(SaveAnomalyCommand request, CancellationToken cancellationToken)
    {
        var saveAnomalyModel = new SaveAnomalyModel(
            request.GoodId,
            request.Price);

        var anomalyId = await _anomalyService.SaveAnomaly(
            saveAnomalyModel,
            cancellationToken);

        return anomalyId;
    }
}