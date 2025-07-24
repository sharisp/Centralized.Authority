using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class ChangePwdDtoValidator : AbstractValidator<ChangePwdDto>
    {
        public ChangePwdDtoValidator()
        {

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("OldPassword is required.")
                .MinimumLength(4).WithMessage("OldPassword must be at least 4 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("NewPassword is required.")
                .MinimumLength(4).WithMessage("NewPassword must be at least 4 characters.");


            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("ConfirmNewPassword is required.")
                .MinimumLength(4).WithMessage("ConfirmNewPassword must be at least 4 characters.")
                .Must((dto, confirmNewPassword) => confirmNewPassword == dto.NewPassword).WithMessage("ConfirmNewPassword must be same with NewPassword");
        }
    }
}
