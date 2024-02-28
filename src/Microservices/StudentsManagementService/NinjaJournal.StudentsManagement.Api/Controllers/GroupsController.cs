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
public class GroupsController : BaseController<Guid, Group, GroupDto, GroupDto>
{
    public GroupsController(ILogger<BaseController<Guid, Group, GroupDto, GroupDto>> logger, 
        IReadEntityRepository<Guid, Group> readEntityRepository, IEntityRepository<Guid, Group> entityRepository, 
        IValidator<GroupDto> validator, IValidator<GroupDto> createValidator, IRedisCacheService redisCacheService, 
        IMapper mapper) 
        : base(logger, readEntityRepository, entityRepository, validator, createValidator, redisCacheService, mapper)
    { }
}