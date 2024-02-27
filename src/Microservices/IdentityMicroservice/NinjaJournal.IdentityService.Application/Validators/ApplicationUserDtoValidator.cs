using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using FluentValidation;

namespace NinjaJournal.IdentityService.Application.Validators;

public class ApplicationUserDtoValidator : AbstractValidator<ApplicationUserDto>
{
    public ApplicationUserDtoValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress()
            .WithMessage("A valid {PropertyName} is required.");
        
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
        
        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
        
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 30).WithMessage("{PropertyName} must be between 2 and 50 characters.");
    }
}