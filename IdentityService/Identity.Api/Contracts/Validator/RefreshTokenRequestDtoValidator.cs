using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {

            RuleFor(x => x.RefreshToken).NotNull()
                .NotEmpty().WithMessage("RefreshToken is required.")
                .Length(36).WithMessage("RefreshToken not correct");
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId is required and must be greater than 0.");
        }
    }
}

