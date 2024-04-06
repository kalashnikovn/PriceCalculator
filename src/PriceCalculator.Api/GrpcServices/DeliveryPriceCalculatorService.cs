using Grpc.Core;
using MediatR;
using PriceCalculator.Api.Responses.V1;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.Api.GrpcServices;

public sealed class DeliveryPriceCalculatorService : DeliveryPriceCalculator.DeliveryPriceCalculatorBase
{
    private readonly IMediator _mediator;

    public DeliveryPriceCalculatorService(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<CalculateResponseMessage> Calculate(
        CalculateRequestMessage request,
        ServerCallContext context)
    {
        var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight
                ))
                .ToArray());

        var result = await _mediator.Send(command, context.CancellationToken);

        return new CalculateResponseMessage
        {
            CalculationId = result.CalculationId,
            Price = DecimalValue.FromDecimal(result.Price)
        };
    }

    public override Task<ClearHistoryResponseMessage> ClearHistory(ClearHistoryRequestMessage request, ServerCallContext context)
    {
        return base.ClearHistory(request, context);
    }
}