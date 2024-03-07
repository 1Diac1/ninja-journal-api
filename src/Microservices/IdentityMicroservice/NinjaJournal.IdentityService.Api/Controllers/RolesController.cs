using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.Microservice.Api.AspNetCore.Options;
using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Extensions;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.Microservice.Core.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Ardalis.GuardClauses;
using FluentValidation;
using AutoMapper;
using NinjaJournal.Microservice.Api.AspNetCore.Contracts;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : BaseController<Guid, ApplicationRole, ApplicationRoleDto, CreateApplicationRoleDto>
{
    private readonly IRoleManager _roleManager;

    public RolesController(ILogger<BaseController<Guid, ApplicationRole, ApplicationRoleDto, CreateApplicationRoleDto>> logger, 
        IReadEntityRepository<Guid, ApplicationRole> readEntityRepository, IEntityRepository<Guid, ApplicationRole> entityRepository, 
        IValidator<CreateApplicationRoleDto> createValidator, IRedisCacheService redisCacheService, IValidator<ApplicationRoleDto> validator,
        ICacheKeyService cacheKeyService, IOptions<CacheOptions> cacheOptions, IMapper mapper, IRoleManager roleManager)
        : base(logger, readEntityRepository, entityRepository, createValidator, redisCacheService, validator, cacheKeyService, cacheOptions, mapper)
    {
        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        
        _roleManager = roleManager;
    }
    
    [HttpPost]
    public override async Task<BaseResponse> CreateAsync(CreateApplicationRoleDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await CreateValidator.ValidateAsync(entityDto, cancellationToken);

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

        var result = await _roleManager.UpdateAsync(entityToUpdate!, cancellationToken);
        result.Check();

        Logger.LogInformation(SuccessMessages.EntityUpdated<Guid, ApplicationRole>(entityToUpdate!.Id));
        
        var cacheKey = CacheKeyService.GenerateCacheKey<Guid, ApplicationRole>(CacheKeyRoutes.Get, entityDto.Id); 
        var cachedEntity = await RedisCacheService.GetAsync<ApplicationRoleDto>(cacheKey, cancellationToken);

        if (cachedEntity is null)
            return BaseResponse.Success();
        
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        Logger.LogInformation(SuccessMessages.DataDeletedFromCache<Guid, ApplicationRole>(entityDto.Id, cacheKey));
        
        return BaseResponse.Success();
    }
}