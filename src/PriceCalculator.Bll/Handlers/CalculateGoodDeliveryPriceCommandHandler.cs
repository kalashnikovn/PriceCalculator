using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.Bll.Handlers;

public sealed class CalculateGoodDeliveryPriceCommandHandler
    : IRequestHandler<CalculateGoodDeliveryPriceCommand, CalculateGoodDeliveryPriceResult>
{
    private readonly ICalculationService _calculationService;

    public CalculateGoodDeliveryPriceCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public Task<CalculateGoodDeliveryPriceResult> Handle(
        CalculateGoodDeliveryPriceCommand request,
        CancellationToken cancellationToken)
    {
        var volumePrice = _calculationService.CalculatePriceByVolume(new[] { request.Good }, out _);
        var weightPrice = _calculationService.CalculatePriceByWeight(new[] { request.Good }, out _);
        var resultPrice = Math.Max(volumePrice, weightPrice);

        return Task.FromResult(new CalculateGoodDeliveryPriceResult(resultPrice));
    }
}