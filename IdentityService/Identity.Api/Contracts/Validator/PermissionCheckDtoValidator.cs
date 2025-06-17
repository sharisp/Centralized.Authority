using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class PermissionCheckDtoValidator : FluentValidation.AbstractValidator<PermissionCheckDto>
    {
        public PermissionCheckDtoValidator()
        {

            RuleFor(x => x.SystemName)
                .NotEmpty().WithMessage("SystemName is required.")
                .Length(3, 50).WithMessage("SystemName must be between 3 and 50 characters.")
                .Must(t => t.Contains(".") == false).WithMessage("SystemName must not contain ."); ;

            RuleFor(x => x.PermissionKey)
                .NotEmpty().WithMessage("PermissionKey is required.")
                .Length(3, 50).WithMessage("PermissionKey must be between 3 and 50 characters.");


            RuleFor(x => x.UserId).Must(t => t > 0).WithMessage("userid must bigger than 0");
        }
    }
}
