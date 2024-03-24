using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Extensions;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.Bll.Handlers;

public sealed class CalculateDeliveryPriceCommandHandler 
    : IRequestHandler<CalculateDeliveryPriceCommand, CalculateDeliveryPriceResult>
{
    private readonly ICalculationService _calculationService;

    public CalculateDeliveryPriceCommandHandler(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    
    public async Task<CalculateDeliveryPriceResult> Handle(
        CalculateDeliveryPriceCommand request,
        CancellationToken cancellationToken)
    {
        request.EnsureHasGoods();

        var volumePrice = _calculationService.CalculatePriceByVolume(request.Goods, out var volume);
        var weightPrice = _calculationService.CalculatePriceByWeight(request.Goods, out var weight);
        var resultPrice = Math.Max(volumePrice, weightPrice);

        var model = new SaveCalculationModel(
            UserId: request.UserId,
            TotalVolume: volume,
            TotalWeight: weight,
            Price: resultPrice,
            Goods: request.Goods
        );
        
        var calculationId = await _calculationService.SaveCalculation(model, cancellationToken);

        return new CalculateDeliveryPriceResult(
            calculationId,
            resultPrice);

    }
}