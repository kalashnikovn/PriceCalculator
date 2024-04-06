using FluentValidation;
using PriceCalculator.Api.Requests.V1;

namespace PriceCalculator.Api.Validators.V1;

public sealed class ClearHistoryRequestValidator : AbstractValidator<ClearHistoryRequest>
{
    public ClearHistoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        
    }   
}