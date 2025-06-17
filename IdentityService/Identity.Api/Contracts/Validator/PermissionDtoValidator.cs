using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class CreatePermissionDtoValidator : FluentValidation.AbstractValidator<CreatePermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 50).WithMessage("Title must be between 3 and 50 characters.");
            RuleFor(x => x.SystemName)
                .NotEmpty().WithMessage("SystemName is required.")
                .Length(3, 50).WithMessage("SystemName must be between 3 and 50 characters.")
                .Must(t=>t.Contains(".")==false).WithMessage("no . allowed");

            RuleFor(x => x.PermissionKey)
                .NotEmpty().WithMessage("PermissionKey is required.")
                .Length(3, 50).WithMessage("Title must be between 3 and 50 characters.");


            RuleFor(x => x.Description)
              //  .NotEmpty().WithMessage("Description is required.")
                .Length(3, 50).WithMessage("Description must be between 3 and 50 characters.");
        }
    }
}
