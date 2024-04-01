using MediatR;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.Bll.Commands;

public record CalculateGoodDeliveryPriceCommand(
    GoodModel Good
    ) : IRequest<CalculateGoodDeliveryPriceResult>;