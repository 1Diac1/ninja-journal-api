using System.Text.Json.Serialization;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using NinjaJournal.StudentsManagement.Domain.Entities;

namespace NinjaJournal.StudentsManagement.Application.Dtos;

public class StudentDto : BaseEntityDto<Guid>
{
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public bool IsHeadOfGroup { get; set; }
    public Guid GroupId { get; set; }
    
    [JsonIgnore]
    public GroupDto Group { get; set; }
}