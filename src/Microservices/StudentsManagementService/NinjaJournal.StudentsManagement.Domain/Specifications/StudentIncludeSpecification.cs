using Ardalis.Specification;
using NinjaJournal.StudentsManagement.Domain.Entities;

namespace NinjaJournal.StudentsManagement.Domain.Specifications;

public sealed class StudentIncludeSpecification : Specification<Student>
{
    public StudentIncludeSpecification()
    {
        Query.Include(s => s.Group);
    }
}