﻿namespace PriceCalculator.Api.Requests.V3;

public record CalculateRequest(
    GoodProperties[] Goods,
    decimal Distance
    );