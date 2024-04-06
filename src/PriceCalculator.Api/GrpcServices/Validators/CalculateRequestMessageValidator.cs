using FluentValidation;

namespace PriceCalculator.Api.GrpcServices.Validators;

public sealed class CalculateRequestMessageValidator : AbstractValidator<CalculateRequestMessage>
{
    public CalculateRequestMessageValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Goods)
            .NotEmpty();

        RuleForEach(x => x.Goods)
            .SetValidator(new GoodPropertiesValidator());
    }
}