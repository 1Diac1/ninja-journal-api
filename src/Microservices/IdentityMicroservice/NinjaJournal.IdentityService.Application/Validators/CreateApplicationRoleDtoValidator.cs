using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using FluentValidation;

namespace NinjaJournal.IdentityService.Application.Validators;

public class CreateApplicationRoleDtoValidator : AbstractValidator<CreateApplicationRoleDto>
{
    public CreateApplicationRoleDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
    }
}