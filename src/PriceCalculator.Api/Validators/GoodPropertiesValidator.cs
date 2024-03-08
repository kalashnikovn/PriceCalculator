using FluentValidation;
using PriceCalculator.Api.Requests.V2;

namespace PriceCalculator.Api.Validators;

public class GoodPropertiesValidator : AbstractValidator<GoodProperties>
{
    public GoodPropertiesValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
        
        RuleFor(x => x.Height)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
        
        RuleFor(x => x.Length)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);

        RuleFor(x => x.Width)
            .GreaterThan(0)
            .LessThan(Int32.MaxValue);
    }
}