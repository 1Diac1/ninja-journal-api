using Ardalis.Specification;
using NinjaJournal.StudentsManagement.Domain.Entities;

namespace NinjaJournal.StudentsManagement.Infrastructure.Postgresql.Specifications;

public sealed class StudentIncludeSpecification : Specification<Student>
{
    public StudentIncludeSpecification()
    {
        Query.Include(s => s.Group);
    }
}