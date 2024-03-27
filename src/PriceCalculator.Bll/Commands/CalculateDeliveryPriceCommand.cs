using MediatR;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.Bll.Commands;

public record CalculateDeliveryPriceCommand(
    long UserId,
    GoodModel[] Goods) : IRequest<CalculateDeliveryPriceResult>;