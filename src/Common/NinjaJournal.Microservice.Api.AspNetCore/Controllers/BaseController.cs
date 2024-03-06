using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore.Specifications;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.Microservice.Api.AspNetCore.Contracts;
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
public abstract class BaseController<TKey, TEntity, TEntityDto, TEntityCreateDto> : ControllerBase
    where TKey : struct
    where TEntity : IAggregateRoot<TKey>, ICacheableEntity<TKey>
    where TEntityDto : BaseEntityDto<TKey>
    where TEntityCreateDto : BaseEntityDto<TKey>
{
    protected readonly ILogger<BaseController<TKey, TEntity, TEntityDto, TEntityCreateDto>> Logger;
    protected readonly IReadEntityRepository<TKey, TEntity> ReadEntityRepository;
    protected readonly IEntityRepository<TKey, TEntity> EntityRepository;
    protected readonly IList<ISpecification<TEntity>> Specifications;
    protected readonly IValidator<TEntityCreateDto> CreateValidator;
    protected readonly IRedisCacheService RedisCacheService;
    protected readonly IValidator<TEntityDto> Validator;
    protected readonly ICacheKeyService CacheKeyService;
    protected readonly IMapper Mapper;


    protected BaseController(ILogger<BaseController<TKey, TEntity, TEntityDto, TEntityCreateDto>> logger, 
        IReadEntityRepository<TKey, TEntity> readEntityRepository, IEntityRepository<TKey, TEntity> entityRepository, 
        IValidator<TEntityCreateDto> createValidator, IRedisCacheService redisCacheService, 
        IValidator<TEntityDto> validator, ICacheKeyService cacheKeyService, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));
        ArgumentNullException.ThrowIfNull(cacheKeyService, nameof(cacheKeyService));
        ArgumentNullException.ThrowIfNull(createValidator, nameof(createValidator));
        ArgumentNullException.ThrowIfNull(entityRepository, nameof(entityRepository));
        ArgumentNullException.ThrowIfNull(redisCacheService, nameof(redisCacheService));
        ArgumentNullException.ThrowIfNull(readEntityRepository, nameof(readEntityRepository));
        
        Logger = logger;
        Mapper = mapper;
        Validator = validator;
        CacheKeyService = cacheKeyService;
        CreateValidator = createValidator;
        EntityRepository = entityRepository;
        RedisCacheService = redisCacheService;
        ReadEntityRepository = readEntityRepository;
        Specifications = new List<ISpecification<TEntity>>();
    }

    [HttpGet]
    public virtual async Task<DataResponse<IReadOnlyCollection<TEntityDto>>> GetAllAsync(int limit, CancellationToken cancellationToken)
    {
        Guard.Against.OutOfRange(limit, nameof(limit), 1, 10000);

        var cacheKey = CacheKeyService.GenerateCacheKey<TKey, TEntity>(CacheKeyRoutes.GetAll, limit); 
        var cachedEntities = await RedisCacheService.GetAsync<IReadOnlyCollection<TEntityDto>>(cacheKey, cancellationToken);

        if (cachedEntities is not null)
        {
            Logger.LogInformation($"Data was retrieved from the cache: {cacheKey}");
            return DataResponse<IReadOnlyCollection<TEntityDto>>.Success(cachedEntities);
        }
        
        Specifications.Add(new EntityWithLimitSpecification<TEntity>(limit));
        
        var entities = await ReadEntityRepository.GetAllAsync(Specifications, true, cancellationToken);
        var mappedEntities = Mapper.Map<IReadOnlyCollection<TEntityDto>>(entities);

        await RedisCacheService.SetAsync(cacheKey, mappedEntities, TimeSpan.FromSeconds(7200), cancellationToken);
        
        return DataResponse<IReadOnlyCollection<TEntityDto>>.Success(mappedEntities);
    }

    [HttpGet("{id}")]
    public virtual async Task<DataResponse<TEntityDto>> GetAsync(TKey id, CancellationToken cancellationToken)
    { 
        Guard.Against.NullOrEmpty(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);
        
        var cacheKey = CacheKeyService.GenerateCacheKey<TKey, TEntity>(CacheKeyRoutes.Get, id); 
        var cachedEntity = await RedisCacheService.GetAsync<TEntityDto>(cacheKey, cancellationToken);

        if (cachedEntity is not null)
        {
            Logger.LogInformation($"Data was retrieved from the cache: {cacheKey}");
            return DataResponse<TEntityDto>.Success(cachedEntity);
        }

        var entity = await ReadEntityRepository.GetByIdAsync(id, true, cancellationToken);
        Guard.Against.NotFoundEntity(id, entity);
        
        var mappedEntity = Mapper.Map<TEntityDto>(entity);
        await RedisCacheService.SetAsync(cacheKey, mappedEntity, TimeSpan.FromSeconds(7200), cancellationToken);
        
        return DataResponse<TEntityDto>.Success(mappedEntity);
    }

    [HttpPost]
    public virtual async Task<BaseResponse> CreateAsync([FromBody] TEntityCreateDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await CreateValidator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);
    
        var mappedEntity = Mapper.Map<TEntity>(entityDto);
        await EntityRepository.AddAsync(mappedEntity, true, cancellationToken);
        
        Logger.LogInformation(SuccessMessages.EntityCreated<TKey, TEntity>(mappedEntity.Id));

        return BaseResponse.Success();
    }
    
    [HttpPut]
    public virtual async Task<BaseResponse> UpdateAsync([FromBody] TEntityDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(entityDto.Id, nameof(entityDto.Id), ErrorMessages.CantBeNullOrEmpty);
        
        var validationResult = await Validator.ValidateAsync(entityDto, cancellationToken);

        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var entityToUpdate = await ReadEntityRepository.GetByIdAsync(entityDto.Id, true, cancellationToken);
        Guard.Against.NotFoundEntity(entityDto.Id, entityToUpdate);

        Mapper.Map(entityDto, entityToUpdate);
        await EntityRepository.UpdateAsync(entityToUpdate, true, cancellationToken);

        Logger.LogInformation(SuccessMessages.EntityUpdated<TKey, TEntity>(entityToUpdate.Id));

        var cacheKey = CacheKeyService.GenerateCacheKey<TKey, TEntity>(CacheKeyRoutes.Get, entityDto.Id); 
        var cachedEntity = await RedisCacheService.GetAsync<TEntityDto>(cacheKey, cancellationToken);

        if (cachedEntity is null)
            return BaseResponse.Success();
        
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        Logger.LogInformation($"Data was deleted from the cache: {cacheKey}");
        
        return DataResponse<TEntityDto>.Success(cachedEntity);
    }

    [HttpDelete("{id}")]
    public virtual async Task<BaseResponse> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);

        var entity = await ReadEntityRepository.GetByIdAsync(id, true, cancellationToken);
        Guard.Against.NotFoundEntity(id, entity);

        await EntityRepository.DeleteAsync(entity, true, cancellationToken);

        Logger.LogInformation(SuccessMessages.EntityDeleted<TKey, TEntity>(id));
        
        var cacheKey = CacheKeyService.GenerateCacheKey<TKey, TEntity>(CacheKeyRoutes.Get, id); 
        var cachedEntity = await RedisCacheService.GetAsync<TEntityDto>(cacheKey, cancellationToken);

        if (cachedEntity is null)
            return BaseResponse.Success();
        
        await RedisCacheService.InvalidateAsync(cacheKey, cancellationToken);
        
        Logger.LogInformation($"Data was deleted from the cache: {cacheKey}");
        
        return DataResponse<TEntityDto>.Success(cachedEntity);
    }
}