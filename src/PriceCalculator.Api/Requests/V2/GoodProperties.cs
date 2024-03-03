﻿namespace PriceCalculator.Api.Requests.V2;

public record GoodProperties(
    decimal Length,
    decimal Width,
    decimal Height,
    decimal Weight
    );