using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserValidator()
        {

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

           /* RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");*/
        }
    }
}
