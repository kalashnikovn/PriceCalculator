using FluentValidation;
using PriceCalculator.BackgroundServices.Messages;

namespace PriceCalculator.BackgroundServices.Validators;

public sealed class CalculateRequestMessageValidator : AbstractValidator<CalculateRequestMessage>
{
    public CalculateRequestMessageValidator()
    {
        RuleFor(x => x.GoodId)
            .GreaterThan(0);
        
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