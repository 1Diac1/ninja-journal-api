using NinjaJournal.StudentsManagement.Application.Dtos.Student;
using NinjaJournal.StudentsManagement.Domain.Entities;
using AutoMapper;

namespace NinjaJournal.StudentsManagement.Application.Mappings;

public class StudentMappings : Profile
{
    public StudentMappings()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentDto, Student>();
    }
}