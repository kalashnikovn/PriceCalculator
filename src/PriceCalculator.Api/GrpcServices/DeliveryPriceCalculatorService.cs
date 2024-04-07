using Grpc.Core;
using MediatR;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Queries;

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

    public override async Task<ClearHistoryResponseMessage> ClearHistory(ClearHistoryRequestMessage request, ServerCallContext context)
    {
        var command = new ClearCalculationsHistoryCommand(
            request.UserId,
            request.CalculationIds.ToArray()
        );

        await _mediator.Send(command, context.CancellationToken);

        return new ClearHistoryResponseMessage();
    }

    public override async Task GetHistory(
        GetHistoryRequestMessage request,
        IServerStreamWriter<GetHistoryResponseMessage> responseStream,
        ServerCallContext context)
    {
        var command = new GetCalculationHistoryQuery(
            request.UserId,
            request.Take,
            0,
            Array.Empty<long>()
        );

        var result = await _mediator.Send(command, context.CancellationToken);
        
        foreach (var historyItem in result.Items)
        {
            await responseStream.WriteAsync(new GetHistoryResponseMessage
            {
                Cargo = new CargoResponse
                {
                    Volume = historyItem.Volume,
                    Weight = historyItem.Weight,
                    GoodIds = { historyItem.GoodIds }
                },
                Price = DecimalValue.FromDecimal(historyItem.Price)
            });
        }
        
    }

    public override async Task CalculateDuplex(
        IAsyncStreamReader<CalculateRequestMessage> requestStream,
        IServerStreamWriter<CalculateResponseMessage> responseStream,
        ServerCallContext context)
    {
        await foreach (var calculateRequestMessage in requestStream.ReadAllAsync())
        {
            var command = new CalculateDeliveryPriceCommand(
                calculateRequestMessage.UserId,
                calculateRequestMessage.Goods
                    .Select(x => new GoodModel(
                        x.Height,
                        x.Length,
                        x.Width,
                        x.Weight
                    ))
                    .ToArray());

            var result = await _mediator.Send(command, context.CancellationToken);

            await responseStream.WriteAsync(new CalculateResponseMessage
            {
                CalculationId = result.CalculationId,
                Price = DecimalValue.FromDecimal(result.Price)
            });
        }
    }
}