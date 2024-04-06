using FluentValidation;

namespace PriceCalculator.Api.GrpcServices.Validators;

public sealed class GetHistoryRequestMessageValidator : AbstractValidator<GetHistoryRequestMessage>
{
    public GetHistoryRequestMessageValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Take)
            .GreaterThan(0);
    }
}