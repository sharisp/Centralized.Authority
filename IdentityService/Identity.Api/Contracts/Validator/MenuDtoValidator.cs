using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;

namespace Identity.Api.Contracts.Validator
{
    public class CreateMenuDtoValidator : FluentValidation.AbstractValidator<CreateMenuDto>
    {
        public CreateMenuDtoValidator()
        {
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 50).WithMessage("Title must be between 3 and 50 characters.");
            RuleFor(x => x.SystemName)
                .NotEmpty().WithMessage("SystemName is required.")
                .Length(3, 50).WithMessage("SystemName must be between 3 and 50 characters.")
                .Must(t=>t.Contains(".")==false).WithMessage("no . allowed");

            

         
        }
    }
}
