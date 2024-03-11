﻿namespace PriceCalculator.Api.Requests.V1;

/// <summary>
/// Харектеристики товара
/// </summary>
public record GoodProperties(
    decimal Height,
    decimal Length,
    decimal Width
    );