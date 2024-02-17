using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;

namespace NinjaJournal.StudentsManagement.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StudentsController : BaseController<Guid, Student, StudentDto>
{
    public StudentsController(IReadEntityRepository<Guid, Student> readEntityRepository,
        ILogger<BaseController<Guid, Student, StudentDto>> logger, 
        IEntityRepository<Guid, Student> entityRepository, 
        IValidator<StudentDto> validator, IMapper mapper) 
        : base(readEntityRepository, logger, entityRepository, validator, mapper)
    { }
    
    [HttpGet]
    [Route("get-something")]
    public string GetSomethingString()
    {
        return "Something string";
    }
}