using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using FluentValidation;

namespace NinjaJournal.IdentityService.Application.Validators;

public class ChangeUserPasswordDtoValidator : AbstractValidator<ChangeUserPasswordDto<Guid>>
{
    public ChangeUserPasswordDtoValidator()
    {
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 30).WithMessage("{PropertyName} must be between 2 and 50 characters.");
        
        RuleFor(u => u.OldPassword)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 30).WithMessage("{PropertyName} must be between 2 and 50 characters.");

        RuleFor(u => u.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}