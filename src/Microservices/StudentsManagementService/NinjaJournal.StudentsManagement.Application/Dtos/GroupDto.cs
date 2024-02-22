using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.StudentsManagement.Application.Dtos;

public class GroupDto : BaseEntityDto<Guid>
{
    public string GroupName { get; set; }
    public int CourseNumber { get; set; }
}