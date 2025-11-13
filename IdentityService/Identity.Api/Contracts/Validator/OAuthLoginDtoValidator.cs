using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class OAuthLoginDtoValidator : AbstractValidator<OAuthLoginDto>
    {
        public OAuthLoginDtoValidator()
        {

            RuleFor(x => x.Provider).IsInEnum().WithMessage("Invalid provider specified.")
                .NotEmpty().WithMessage("Provider is required.");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
        }
    }
}
