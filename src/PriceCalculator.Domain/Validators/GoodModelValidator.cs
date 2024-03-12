using FluentValidation;
using PriceCalculator.Domain.Models.PriceCalculator;

namespace PriceCalculator.Domain.Validators;

public sealed class GoodModelValidator : AbstractValidator<GoodModel>
{
    public GoodModelValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
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