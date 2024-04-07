using FluentValidation;
using PriceCalculator.Api.Requests.V1;

namespace PriceCalculator.Api.Validators.V1;

public sealed class GetHistoryRequestValidator : AbstractValidator<GetHistoryRequest>
{
    public GetHistoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Take)
            .GreaterThan(0);
    }
}