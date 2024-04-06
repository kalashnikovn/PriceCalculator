using FluentValidation;

namespace PriceCalculator.Api.GrpcServices.Validators;

public sealed class ClearHistoryRequestMessageValidator : AbstractValidator<ClearHistoryRequestMessage>
{
    public ClearHistoryRequestMessageValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
    }
}