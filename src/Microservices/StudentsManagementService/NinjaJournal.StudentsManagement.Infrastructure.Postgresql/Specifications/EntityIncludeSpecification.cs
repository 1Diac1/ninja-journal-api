using Ardalis.Specification;
using NinjaJournal.StudentsManagement.Domain.Entities;

namespace NinjaJournal.StudentsManagement.Infrastructure.Postgresql.Specifications;

public sealed class EntityIncludeSpecification : Specification<Student>
{
    public EntityIncludeSpecification()
    {
        Query.Include(s => s.Group);
    }
}