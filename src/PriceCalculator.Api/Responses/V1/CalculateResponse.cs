﻿namespace PriceCalculator.Api.Responses.V1;

public record CalculateResponse(
    long CalculationId,
    decimal Price
);
    