using FluentValidation;
using PriceCalculator.Domain.Models.PriceCalculator;

namespace PriceCalculator.Domain.Validators;

public sealed class GoodsValidator : AbstractValidator<IReadOnlyCollection<GoodModel>>
{
    public GoodsValidator()
    {
        RuleForEach(x => x)
            .SetValidator(new GoodModelValidator());
        
        RuleFor(x => x)
            .Must(x => x.Any());
    }
}