using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore.Specifications;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.Microservice.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using FluentValidation;
using AutoMapper;

namespace NinjaJournal.Microservice.Api.AspNetCore.Controllers;

[ApiController]
[AllowAnonymous]
public abstract class BaseController<TKey, TEntity, TEntityDto> : ControllerBase
    where TKey : struct
    where TEntity : BaseEntity<TKey>
    where TEntityDto : BaseEntityDto<TKey>
{
    protected readonly ILogger<BaseController<TKey, TEntity, TEntityDto>> Logger;
    protected readonly IReadEntityRepository<TKey, TEntity> ReadEntityRepository;
    protected readonly IEntityRepository<TKey, TEntity> EntityRepository;
    protected readonly IList<ISpecification<TEntity>> Specifications;
    protected readonly IValidator<TEntityDto> Validator;
    protected readonly IMapper Mapper;

    protected BaseController(ILogger<BaseController<TKey, TEntity, TEntityDto>> logger,
        IReadEntityRepository<TKey, TEntity> readEntityRepository, IEntityRepository<TKey, TEntity> entityRepository,
        IValidator<TEntityDto> validator, IMapper mapper)
    {
        Logger = logger ?? throw new ArgumentException(nameof(ILogger<BaseController<TKey, TEntity, TEntityDto>>));
        ReadEntityRepository = readEntityRepository ?? throw new ArgumentException(nameof(IReadEntityRepository<TKey, TEntity>));
        EntityRepository = entityRepository ?? throw new ArgumentException(nameof(IEntityRepository<TKey, TEntity>));
        Validator = validator ?? throw new ArgumentException(nameof(IValidator<TEntityDto>));
        Mapper = mapper ?? throw new ArgumentException(nameof(IMapper));
        Specifications = new List<ISpecification<TEntity>>();
    }

    [HttpGet]
    public virtual async Task<DataResponse<IReadOnlyCollection<TEntityDto>>> GetAllAsync(int limit, CancellationToken cancellationToken)
    {
        Guard.Against.OutOfRange(limit, nameof(limit), 1, 10000);

        var entities = await ReadEntityRepository.GetAllAsync(Specifications, true, cancellationToken);
        var mappedEntities = Mapper.Map<IReadOnlyCollection<TEntityDto>>(entities);

        return DataResponse<IReadOnlyCollection<TEntityDto>>.Success(mappedEntities);
    }

    [HttpGet("{id}")]
    public virtual async Task<DataResponse<TEntityDto>> GetAsync(TKey id, CancellationToken cancellationToken)
    {
        Guard.Against.Null(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);
        
        var entity = await ReadEntityRepository.GetByIdAsync(id, true, cancellationToken);

        Guard.Against.NotFoundEntity(id, entity);
        
        var mappedEntity = Mapper.Map<TEntityDto>(entity);

        return DataResponse<TEntityDto>.Success(mappedEntity);
    }

    [HttpPost]
    public virtual async Task<BaseResponse> CreateAsync([FromBody] TEntityDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<TEntity>(entityDto);

        await EntityRepository.AddAsync(mappedEntity, true, cancellationToken);
        
        Logger.LogInformation(SuccessMessages.EntityCreated<TKey, TEntity>(mappedEntity.Id));

        return BaseResponse.Success();
    }
    
    [HttpPut]
    public async Task<BaseResponse> UpdateAsync([FromBody] TEntityDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var entityToUpdate = await ReadEntityRepository.GetByIdAsync(entityDto.Id, true, cancellationToken);

        Guard.Against.NotFoundEntity(entityDto.Id, entityToUpdate);

        Mapper.Map(entityDto, entityToUpdate);

        await EntityRepository.UpdateAsync(entityToUpdate, true, cancellationToken);

        Logger.LogInformation(SuccessMessages.EntityUpdated<TKey, TEntity>(entityToUpdate.Id));
        
        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        Guard.Against.Null(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);

        var entity = await ReadEntityRepository.GetByIdAsync(id, true, cancellationToken);

        Guard.Against.NotFoundEntity(id, entity);

        await EntityRepository.DeleteAsync(entity, true, cancellationToken);

        Logger.LogInformation(SuccessMessages.EntityDeleted<TKey, TEntity>(id));
        
        return BaseResponse.Success();
    }
}