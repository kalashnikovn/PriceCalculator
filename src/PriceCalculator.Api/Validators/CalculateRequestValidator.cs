using FluentValidation;
using PriceCalculator.Api.Requests.V2;

namespace PriceCalculator.Api.Validators;

public class CalculateRequestValidator : AbstractValidator<CalculateRequest>
{
    public CalculateRequestValidator()
    { 
        RuleForEach(x => x.Goods)
            .SetValidator(new GoodPropertiesValidator());
    }
}