using FluentValidation;
using NinjaJournal.IdentityService.Application.Dtos;

namespace NinjaJournal.IdentityService.Application.Validators;

public class ApplicationRoleDtoValidator : AbstractValidator<ApplicationRoleDto>
{
    public ApplicationRoleDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
    }
}