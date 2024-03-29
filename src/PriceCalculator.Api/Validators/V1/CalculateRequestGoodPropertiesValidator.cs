﻿using FluentValidation;
using PriceCalculator.Api.Requests.V1;

namespace PriceCalculator.Api.Validators.V1;

public class CalculateRequestGoodPropertiesValidator : AbstractValidator<CalculateRequest.GoodProperties>
{
    public CalculateRequestGoodPropertiesValidator()
    {
        RuleFor(x => x.Height)
            .GreaterThan(0);
        
        RuleFor(x => x.Length)
            .GreaterThan(0);
        
        RuleFor(x => x.Width)
            .GreaterThan(0);
        
        RuleFor(x => x.Weight)
            .GreaterThan(0);
        
    }
}