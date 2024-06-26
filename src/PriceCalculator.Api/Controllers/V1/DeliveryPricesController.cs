﻿using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.ActionFilters;
using PriceCalculator.Api.Requests.V1;
using PriceCalculator.Api.Responses.V1;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Queries;

namespace PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("v1/delivery-prices")]
public sealed class DeliveryPricesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryPricesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(CalculateRequest request, CancellationToken cancellationToken)
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

        var result = await _mediator.Send(command, cancellationToken);

        return new CalculateResponse(
            result.CalculationId,
            result.Price);
    }

    /// <summary>
    /// Метод получения истории вычисления
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public async Task<GetHistoryResponse[]> GetHistory(GetHistoryRequest request, CancellationToken cancellationToken)
    {
        var command = new GetCalculationHistoryQuery(
            request.UserId,
            request.Take,
            request.Skip,
            request.CalculationIds
            );

        var result = await _mediator.Send(command, cancellationToken);

        return result.Items.Select(x =>
                new GetHistoryResponse(
                    new GetHistoryResponse.CargoResponse(
                        x.Volume,
                        x.Weight,
                        x.GoodIds),
                    x.Price)
            ).ToArray();
    }
    
    /// <summary>
    /// Method for clearing the user's settlement history
    /// </summary>
    /// <returns></returns>
    [HttpPost("clear-history")]
    [ClearHistoryExceptionFilter]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(OneOrManyCalculationsBelongsToAnotherUserResponse), (int)HttpStatusCode.Forbidden)]
    public async Task ClearHistory(ClearHistoryRequest request, CancellationToken cancellationToken)
    {
        
        var command = new ClearCalculationsHistoryCommand(
            request.UserId,
            request.CalculationIds
            );

        await _mediator.Send(command, cancellationToken);
    }
}