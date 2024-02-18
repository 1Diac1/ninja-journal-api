using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.StudentsManagement.Domain.Entities;

public class Student : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public bool IsHeadOfGroup { get; set; }
    public Guid GroupId { get; set; }

    public Group Group { get; set; }

    public Student()
    {
        Id = Guid.NewGuid();
    }
}