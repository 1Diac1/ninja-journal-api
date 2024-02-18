using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.StudentsManagement.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class GroupsController : BaseController<Guid, Group, GroupDto>
{
    public GroupsController(IReadEntityRepository<Guid, Group> readEntityRepository,
        ILogger<BaseController<Guid, Group, GroupDto>> logger, 
        IEntityRepository<Guid, Group> entityRepository, 
        IValidator<GroupDto> validator, IMapper mapper) 
        : base(readEntityRepository, logger, entityRepository, validator, mapper)
    { }
    
    
}