using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAuth.Entities.DTO;

namespace Identity.Api.Contracts.Validator
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {

            RuleFor(x => x.RoleName).NotNull()
                .NotEmpty().WithMessage("RoleName is required.")
                .Length(3, 50).WithMessage("RoleName must be between 3 and 50 characters.");
        }
    }
}
