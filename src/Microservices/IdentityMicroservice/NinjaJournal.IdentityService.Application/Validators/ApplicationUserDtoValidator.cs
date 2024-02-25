using NinjaJournal.IdentityService.Application.Dtos;
using FluentValidation;

namespace NinjaJournal.IdentityService.Application.Validators;

public class ApplicationUserDtoValidator : AbstractValidator<ApplicationUserDto>
{
    public ApplicationUserDtoValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
    }
}