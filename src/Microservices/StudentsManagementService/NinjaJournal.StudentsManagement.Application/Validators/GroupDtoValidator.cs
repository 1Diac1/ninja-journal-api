using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using FluentValidation;

namespace NinjaJournal.StudentsManagement.Application.Validators;

public class GroupDtoValidator : AbstractValidator<GroupDto>
{
    private readonly IReadEntityRepository<Guid, Group> _readGroupEntityRepository;
    
    public GroupDtoValidator(IReadEntityRepository<Guid, Group> readGroupEntityRepository)
    {
        _readGroupEntityRepository = readGroupEntityRepository;

        RuleFor(group => group.GroupName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.")
            .MustAsync(async (groupName, cancellationToken) => await IsGroupNameExistsAsync(groupName, cancellationToken))
            .WithMessage("Group with this name already exists");;
        
        RuleFor(group => group.CourseNumber)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .InclusiveBetween(1, 4).WithMessage("{PropertyName} must be between 1 and 4.");
    }
    
    private async Task<bool> IsGroupNameExistsAsync(string groupName, CancellationToken cancellationToken)
    {
        var group = await _readGroupEntityRepository.GetAsync(g => g.GroupName == groupName, true, cancellationToken);

        return group is null;
    }
}