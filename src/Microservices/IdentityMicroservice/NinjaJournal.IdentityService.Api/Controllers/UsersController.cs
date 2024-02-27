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
public class UsersController : BaseController<Guid, ApplicationUser, ApplicationUserDto>
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    
    public UsersController(ILogger<BaseController<Guid, ApplicationUser, ApplicationUserDto>> logger, 
        IReadEntityRepository<Guid, ApplicationUser> readEntityRepository, IEntityRepository<Guid, ApplicationUser> entityRepository, 
        IValidator<ApplicationUserDto> validator, IRedisCacheService redisCacheService, IMapper mapper, 
        IUserManager userManager, IRoleManager roleManager)
        : base(logger, readEntityRepository, entityRepository, validator, redisCacheService, mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    public override async Task<BaseResponse> CreateAsync(ApplicationUserDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<ApplicationUser>(entityDto);

        var result = await _userManager.CreateAsync(mappedEntity, entityDto.Password, cancellationToken);

        result.Check();

        Logger.LogInformation(SuccessMessages.EntityCreated<Guid, ApplicationRole>(mappedEntity.Id));

        return BaseResponse.Success();
    }

    [HttpPost]
    [Route(ApiRoutes.IdentityService.RemoveRoleFromUser)]
    public async Task<BaseResponse> RemoveRoleFromUserAsync(RemoveRoleFromUserDto<Guid, Guid> request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(request.RoleId, nameof(request.RoleId), ErrorMessages.CantBeNullOrEmpty);

        var user = await _userManager.FindByIdAsync(request.UserId, cancellationToken);

        Guard.Against.NotFoundEntity(request.UserId, user);

        var role = await _roleManager.FindByIdAsync(request.RoleId, cancellationToken);

        Guard.Against.NotFoundEntity(request.RoleId, role);

        await _userManager.RemoveFromRoleAsync(user, role.Name, cancellationToken);

        Logger.LogInformation(IdentitySuccessMessages.RoleRemovedFromUser<Guid, Guid>(request.UserId, request.RoleId));
        
        var cacheKey = $"{typeof(ApplicationUser)}:{request.UserId}";
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        return BaseResponse.Success();
    }
}