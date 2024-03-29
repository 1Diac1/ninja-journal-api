﻿using NinjaJournal.StudentsManagement.Infrastructure.Postgresql.Specifications;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.StudentsManagement.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StudentsController : BaseController<Guid, Student, StudentDto>
{
    public StudentsController(ILogger<BaseController<Guid, Student, StudentDto>> logger, 
        IReadEntityRepository<Guid, Student> readEntityRepository, IEntityRepository<Guid, Student> entityRepository, 
        IValidator<StudentDto> validator, IRedisCacheService redisCacheService, IMapper mapper) 
        : base(logger, readEntityRepository, entityRepository, validator, redisCacheService, mapper)
    {
        Specifications.Add(new StudentIncludeSpecification());
    }
}