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
using Microsoft.Extensions.Options;
using NinjaJournal.Microservice.Api.AspNetCore.Options;

namespace NinjaJournal.IdentityService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : BaseController<Guid, ApplicationUser, ApplicationUserDto, CreateApplicationUserDto>
{
    private readonly IValidator<ChangeUserPasswordDto<Guid>> _changePasswordDtoValidator;
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    
    public UsersController(ILogger<BaseController<Guid, ApplicationUser, ApplicationUserDto, CreateApplicationUserDto>> logger,
        IReadEntityRepository<Guid, ApplicationUser> readEntityRepository, IEntityRepository<Guid, ApplicationUser> entityRepository, 
        IValidator<CreateApplicationUserDto> createValidator, IRedisCacheService redisCacheService, 
        IValidator<ApplicationUserDto> validator, ICacheKeyService cacheKeyService, IOptions<CacheOptions> cacheOptions, 
        IMapper mapper, IValidator<ChangeUserPasswordDto<Guid>> changePasswordDtoValidator,
        IUserManager userManager, IRoleManager roleManager) 
        : base(logger, readEntityRepository, entityRepository, createValidator, redisCacheService, validator, cacheKeyService, cacheOptions, mapper)
    {
        ArgumentNullException.ThrowIfNull(changePasswordDtoValidator, nameof(changePasswordDtoValidator));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        
        _changePasswordDtoValidator = changePasswordDtoValidator;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Route(ApiRoutes.IdentityService.GetUserRoles)]
    public async Task<DataResponse<IList<string>>> GetUserRolesAsync(GetUserRolesDto<Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId), ErrorMessages.CantBeNullOrEmpty);

        var user = await _userManager.FindByIdAsync(request.UserId, cancellationToken);
        Guard.Against.NotFoundEntity(request.UserId, user);

        var result = await _userManager.GetRolesAsync(user!, cancellationToken);

        return DataResponse<IList<string>>.Success(result);
    }

    [HttpGet]
    [Route(ApiRoutes.IdentityService.GetUsersInRole)]
    public async Task<DataResponse<IList<ApplicationUserDto>>> GetUsersInRoleAsync(GetUsersInRoleDto<Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.RoleId, nameof(request.RoleId), ErrorMessages.CantBeNullOrEmpty);
        
        var role = await _roleManager.FindByIdAsync(request.RoleId, cancellationToken);
        Guard.Against.NotFoundEntity(request.RoleId, role);

        var result = await _userManager.GetUsersInRoleAsync(role?.Name!, cancellationToken);
        var mappedUsers = Mapper.Map<IList<ApplicationUserDto>>(result);

        return DataResponse<IList<ApplicationUserDto>>.Success(mappedUsers);
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

        var result = await _userManager.UpdateAsync(entityToUpdate!, cancellationToken);
        result.Check();

        Logger.LogInformation(SuccessMessages.EntityUpdated<Guid, ApplicationUser>(entityToUpdate!.Id));
        
        var cacheKey = CacheKeyService.GenerateCacheKey<Guid, ApplicationUser>(CacheKeyRoutes.Get, entityDto.Id); 
        var cachedEntity = await RedisCacheService.GetAsync<ApplicationUserDto>(cacheKey, cancellationToken);

        if (cachedEntity is null)
            return BaseResponse.Success();
        
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        Logger.LogInformation(SuccessMessages.DataDeletedFromCache<Guid, ApplicationUser>(entityDto.Id, cacheKey));
        
        return BaseResponse.Success();
    }

    [HttpPost]
    [Route(ApiRoutes.IdentityService.ChangePassword)]
    public async Task<BaseResponse> ChangePasswordAsync(ChangeUserPasswordDto<Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.OldPassword, nameof(request.OldPassword), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await _changePasswordDtoValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var user = await _userManager.FindByIdAsync(request.UserId, cancellationToken);
        Guard.Against.NotFoundEntity(request.UserId, user);
        
        var result = await _userManager.ChangePasswordAsync(user!, request.OldPassword, request.Password, cancellationToken);
        result.Check();

        Logger.LogInformation(IdentitySuccessMessages.PasswordChanged(request.UserId));

        return BaseResponse.Success();
    }

    [HttpPost]
    [Route(ApiRoutes.IdentityService.AddRoleToUser)]
    public async Task<BaseResponse> AddRoleToUserAsync(AddRoleToUserDto<Guid, Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.RoleId, nameof(request.RoleId), ErrorMessages.CantBeNullOrEmpty);
        
        var user = await _userManager.FindByIdAsync(request.UserId, cancellationToken);
        Guard.Against.NotFoundEntity(request.UserId, user);
        
        var role = await _roleManager.FindByIdAsync(request.RoleId, cancellationToken);
        Guard.Against.NotFoundEntity(request.RoleId, role);

        var result = await _userManager.AddToRoleAsync(user!, role?.Name!, cancellationToken);
        result.Check();

        Logger.LogInformation(IdentitySuccessMessages.AddRoleToUser<Guid, Guid>(request.UserId, request.RoleId));
        
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

        var result = await _userManager.RemoveFromRoleAsync(user!, role?.Name!, cancellationToken);
        result.Check();

        Logger.LogInformation(IdentitySuccessMessages.RoleRemovedFromUser<Guid, Guid>(request.UserId, request.RoleId));

        return BaseResponse.Success();
    }
}