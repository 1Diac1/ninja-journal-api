using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NinjaJournal.IdentityService.Application.Dtos;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Core.Helpers;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : BaseController<Guid, ApplicationRole, ApplicationRoleDto>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RolesController(ILogger<BaseController<Guid, ApplicationRole, ApplicationRoleDto>> logger, 
        IReadEntityRepository<Guid, ApplicationRole> readEntityRepository, IEntityRepository<Guid, ApplicationRole> entityRepository,
        IValidator<ApplicationRoleDto> validator, IRedisCacheService redisCacheService, IMapper mapper, RoleManager<ApplicationRole> roleManager) 
        : base(logger, readEntityRepository, entityRepository, validator, redisCacheService, mapper)
    {
        _roleManager = roleManager ?? throw new ArgumentException(nameof(roleManager));
    }

    [HttpPost]
    public async override Task<BaseResponse> CreateAsync(ApplicationRoleDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<ApplicationRole>(entityDto);

        await _roleManager.CreateAsync(mappedEntity, cancellationToken);
        
        Logger.LogInformation(SuccessMessages.EntityCreated<TKey, TEntity>(mappedEntity.Id));

        return BaseResponse.Success();
    }
}