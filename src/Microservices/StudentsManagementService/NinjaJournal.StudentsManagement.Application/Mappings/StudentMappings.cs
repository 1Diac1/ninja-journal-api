using NinjaJournal.StudentsManagement.Domain.Entities;
using AutoMapper;
using NinjaJournal.StudentsManagement.Application.Dtos;

namespace NinjaJournal.StudentsManagement.Application.Mappings;

public class StudentMappings : Profile
{
    public StudentMappings()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentDto, Student>();
    }
}