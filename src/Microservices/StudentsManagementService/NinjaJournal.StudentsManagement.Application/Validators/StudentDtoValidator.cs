using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using FluentValidation;

namespace NinjaJournal.StudentsManagement.Application.Validators;

public class StudentDtoValidator : AbstractValidator<StudentDto>
{
    private readonly IReadEntityRepository<Guid, Group> _readGroupEntityRepository;
    private readonly IReadEntityRepository<Guid, Student> _readStudentEntityRepository;

    public StudentDtoValidator(IReadEntityRepository<Guid, Group> readEntityRepository, 
        IReadEntityRepository<Guid, Student> readStudentEntityRepository)
    {
        _readGroupEntityRepository = readEntityRepository;
        _readStudentEntityRepository = readStudentEntityRepository;

        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");

        RuleFor(user => user.Lastname)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");

        RuleFor(user => user.MiddleName)
            .Length(0, 50).WithMessage("{PropertyName} must be less than 50 characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress()
            .WithMessage("A valid {PropertyName} is required.")
            .MustAsync(async (email, cancellationToken) => await IsUniqueEmail(email, cancellationToken))
            .WithMessage("Student with this email already exists");

        RuleFor(user => user.GroupId)
            .NotNull().WithMessage("{PropertyName} can't be null")
            .Must(BeValidGuid)
            .WithMessage("{PropertyName} can't be empty")
            .MustAsync(async (id, cancellationToken) => await IsGroupExistsAsync(id, cancellationToken))
            .WithMessage("Group with this id doesn't exist");
    }

    private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var student = await _readStudentEntityRepository
            .GetAsync(user => user.Email == email, true, cancellationToken);

        return student is null;
    }
    
    private async Task<bool> IsGroupExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var group = await _readGroupEntityRepository.GetByIdAsync(id, true, cancellationToken);

        return group is null ? false : true;
    }

    private bool BeValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}