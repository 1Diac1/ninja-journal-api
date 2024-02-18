using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AutoMapper;
using NinjaJournal.StudentsManagement.Application.Dtos.Student;

namespace NinjaJournal.StudentsManagement.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StudentsController : BaseController<Guid, Student, StudentDto, StudentUpdateDto>
{
    public StudentsController(IReadEntityRepository<Guid, Student> readEntityRepository,
        ILogger<BaseController<Guid, Student, StudentDto, StudentUpdateDto>> logger, 
        IEntityRepository<Guid, Student> entityRepository, IValidator<StudentDto> validator, IMapper mapper) 
        : base(readEntityRepository, logger, entityRepository, validator, mapper)
    { }
}