using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.IdentityService.Application.Dtos;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : BaseController<Guid, ApplicationUser, ApplicationUserDto>
{
    public UsersController(ILogger<BaseController<Guid, ApplicationUser, ApplicationUserDto>> logger, 
        IReadEntityRepository<Guid, ApplicationUser> readEntityRepository, IEntityRepository<Guid, ApplicationUser> entityRepository, 
        IValidator<ApplicationUserDto> validator, IRedisCacheService redisCacheService, IMapper mapper)
        : base(logger, readEntityRepository, entityRepository, validator, redisCacheService, mapper)
    { }
    
    
}