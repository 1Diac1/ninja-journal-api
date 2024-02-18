using FluentValidation;
using NinjaJournal.StudentsManagement.Application.Dtos;

namespace NinjaJournal.StudentsManagement.Application.Validators;

public class GroupDtoValidator : AbstractValidator<GroupDto>
{
    public GroupDtoValidator()
    {
        RuleFor(group => group.GroupName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");
        
        RuleFor(group => group.CourseNumber)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .InclusiveBetween(1, 4).WithMessage("{PropertyName} must be between 1 and 4.");
        
        RuleFor(group => group.Id)
            .NotNull().WithMessage("{PropertyName} can't be null")
            .Must(guid => guid != Guid.Empty).WithMessage("{PropertyName} can't be empty");
    }
}