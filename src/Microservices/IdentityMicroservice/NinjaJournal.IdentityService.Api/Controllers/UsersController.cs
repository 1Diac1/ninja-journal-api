using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using NinjaJournal.Microservice.Api.AspNetCore.Controllers;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.Microservice.Api.AspNetCore.Contracts;
using NinjaJournal.IdentityService.Domain.Extensions;
using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.IdentityService.Domain.Helpers;
using NinjaJournal.Microservice.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Ardalis.GuardClauses;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : BaseController<Guid, ApplicationUser, ApplicationUserDto, CreateApplicationUserDto>
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    
    public UsersController(ILogger<BaseController<Guid, ApplicationUser, ApplicationUserDto, CreateApplicationUserDto>> logger, 
        IReadEntityRepository<Guid, ApplicationUser> readEntityRepository, IEntityRepository<Guid, ApplicationUser> entityRepository,
        IValidator<ApplicationUserDto> validator, IValidator<CreateApplicationUserDto> createValidator, 
        IRedisCacheService redisCacheService, IMapper mapper, IUserManager userManager, IRoleManager roleManager) 
        : base(logger, readEntityRepository, entityRepository, validator, createValidator, redisCacheService, mapper)
    {
        _userManager = userManager ?? throw new ArgumentException(nameof(IUserManager));
        _roleManager = roleManager ?? throw new ArgumentException(nameof(IRoleManager));
    }
        
    [HttpPost]
    public override async Task<BaseResponse> CreateAsync(CreateApplicationUserDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await CreateValidator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<ApplicationUser>(entityDto);

        var result = await _userManager.CreateAsync(mappedEntity, entityDto.Password, cancellationToken);

        result.Check();

        Logger.LogInformation(SuccessMessages.EntityCreated<Guid, ApplicationRole>(mappedEntity.Id));

        return BaseResponse.Success();
    }

    [HttpPut]
    public override async Task<BaseResponse> UpdateAsync(ApplicationUserDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(entityDto.Id, nameof(entityDto.Id), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var entityToUpdate = await _userManager.FindByIdAsync(entityDto.Id, cancellationToken);

        Guard.Against.NotFoundEntity(entityDto.Id, entityToUpdate);

        Mapper.Map(entityDto, entityToUpdate);

        var result = await _userManager.UpdateAsync(entityToUpdate, cancellationToken);
        
        result.Check();

        Logger.LogInformation(SuccessMessages.EntityUpdated<Guid, ApplicationUser>(entityToUpdate.Id));
        
        var cacheKey = $"{typeof(ApplicationUser)}:{entityDto.Id}";
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        return BaseResponse.Success();
    }

    [HttpPost]
    [Route(ApiRoutes.IdentityService.RemoveRoleFromUser)]
    public async Task<BaseResponse> RemoveRoleFromUserAsync(RemoveRoleFromUserDto<Guid, Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.RoleId, nameof(request.RoleId), ErrorMessages.CantBeNullOrEmpty);

        var user = await _userManager.FindByIdAsync(request.UserId, cancellationToken);

        Guard.Against.NotFoundEntity(request.UserId, user);

        var role = await _roleManager.FindByIdAsync(request.RoleId, cancellationToken);

        Guard.Against.NotFoundEntity(request.RoleId, role);

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name, cancellationToken);

        result.Check();

        Logger.LogInformation(IdentitySuccessMessages.RoleRemovedFromUser<Guid, Guid>(request.UserId, request.RoleId));
        
        var cacheKey = $"{typeof(ApplicationUser)}:{request.UserId}";
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        return BaseResponse.Success();
    }
}