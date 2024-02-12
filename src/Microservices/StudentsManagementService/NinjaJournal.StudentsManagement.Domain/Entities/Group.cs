using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.StudentsManagement.Domain.Entities;

public class Group : BaseEntity<Guid>
{
    public string GroupName { get; set; }
    public int CourseNumber { get; set; }

    public ICollection<Student> Students { get; set; }

}