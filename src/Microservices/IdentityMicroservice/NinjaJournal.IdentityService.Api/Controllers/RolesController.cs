using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Extensions;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.Microservice.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Ardalis.GuardClauses;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : BaseController<Guid, ApplicationRole, ApplicationRoleDto>
{
    private readonly IRoleManager _roleManager;

    public RolesController(ILogger<BaseController<Guid, ApplicationRole, ApplicationRoleDto>> logger, 
        IReadEntityRepository<Guid, ApplicationRole> readEntityRepository, IEntityRepository<Guid, ApplicationRole> entityRepository,
        IValidator<ApplicationRoleDto> validator, IRedisCacheService redisCacheService, IMapper mapper, IRoleManager roleManager) 
        : base(logger, readEntityRepository, entityRepository, validator, redisCacheService, mapper)
    {
        _roleManager = roleManager ?? throw new ArgumentException(nameof(IRoleManager));
    }

    [HttpPost]
    public override async Task<BaseResponse> CreateAsync(ApplicationRoleDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<ApplicationRole>(entityDto);

        var result = await _roleManager.CreateAsync(mappedEntity, cancellationToken);

        result.Check();

        Logger.LogInformation(SuccessMessages.EntityCreated<Guid, ApplicationRole>(mappedEntity.Id));

        return BaseResponse.Success();
    }

    [HttpPut]
    public override async Task<BaseResponse> UpdateAsync(ApplicationRoleDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(entityDto.Id, nameof(entityDto.Id), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var entityToUpdate = await _roleManager.FindByIdAsync(entityDto.Id, cancellationToken);

        Guard.Against.NotFoundEntity(entityDto.Id, entityToUpdate);

        Mapper.Map(entityDto, entityToUpdate);

        var result = await _roleManager.UpdateAsync(entityToUpdate, cancellationToken);
        
        result.Check();

        Logger.LogInformation(SuccessMessages.EntityUpdated<Guid, ApplicationRole>(entityToUpdate.Id));
        
        var cacheKey = $"{typeof(ApplicationRole)}:{entityDto.Id}";
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        return BaseResponse.Success();
    }
}