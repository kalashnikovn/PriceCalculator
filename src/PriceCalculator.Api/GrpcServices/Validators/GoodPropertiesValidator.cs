using FluentValidation;

namespace PriceCalculator.Api.GrpcServices.Validators;

public sealed class GoodPropertiesValidator : AbstractValidator<GoodProperties>
{
    public GoodPropertiesValidator()
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