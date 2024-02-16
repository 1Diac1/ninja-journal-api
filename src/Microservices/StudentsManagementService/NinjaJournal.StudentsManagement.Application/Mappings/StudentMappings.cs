using AutoMapper;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;

namespace NinjaJournal.StudentsManagement.Application.Mappings;

public class StudentMappings : Profile
{
    public StudentMappings()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentDto, Student>();
    }
}