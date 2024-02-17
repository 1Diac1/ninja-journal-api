using NinjaJournal.StudentsManagement.Application.Dtos;
using FluentValidation;

namespace NinjaJournal.StudentsManagement.Application.Validators;

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    public StudentDtoValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(user => user.Lastname)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(user => user.MiddleName)
            .Length(0, 50).WithMessage("Middle name must be less than 50 characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}