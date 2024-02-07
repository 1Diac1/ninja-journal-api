using NinjaJournal.Microservice.Common.Repositories;
using NinjaJournal.Microservice.Common.Helpers;
using NinjaJournal.Microservice.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ardalis.GuardClauses;
using AutoMapper;
using NinjaJournal.Microservice.Common.Specifications;

namespace NinjaJournal.Microservice.Common.Controllers;

[ApiController]
[AllowAnonymous]
public abstract class BaseController<TEntity, TEntityDto> : ControllerBase
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
{
    protected readonly IEntityRepository<TEntity> EntityRepository;
    protected readonly IMapper Mapper; 

    protected BaseController(IEntityRepository<TEntity> entityRepository, IMapper mapper)
    {
        EntityRepository = entityRepository;
        Mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<DataResponse<IReadOnlyCollection<TEntityDto>>> GetAllAsync(int limit, CancellationToken cancellationToken)
    {
        Guard.Against.OutOfRange(limit, nameof(limit), 1, 10000);
        
        var spec = new EntityWithLimitSpecification<TEntity>(limit);

        var entities = await EntityRepository.GetAllAsync(spec, cancellationToken);
        var mappedEntities = Mapper.Map<IReadOnlyCollection<TEntityDto>>(entities);

        return DataResponse<IReadOnlyCollection<TEntityDto>>.Success(mappedEntities);
    }

    [HttpGet("{id}")]
    public virtual async Task<DataResponse<TEntityDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);
        
        var entity = await EntityRepository.GetByIdAsync(id, cancellationToken);

        Guard.Against.NotFound(entity.Id, entity);
        
        var mappedEntity = Mapper.Map<TEntityDto>(entity);

        return DataResponse<TEntityDto>.Success(mappedEntity);
    }

    [HttpPost]
    public async Task<BaseResponse> CreateAsync([FromBody] TEntityDto entityDto, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);

        var mappedEntity = Mapper.Map<TEntity>(entityDto);

        await EntityRepository.AddAsync(mappedEntity, cancellationToken);

        return BaseResponse.Success();
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateAsync([FromBody] TEntityDto entityDto, Guid id, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entityDto, nameof(entityDto), ErrorMessages.CantBeNullOrEmpty);
        Guard.Against.NullOrEmpty(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);

        var entityToUpdate = await EntityRepository.GetByIdAsync(id, cancellationToken);

        Guard.Against.Null(entityToUpdate);

        Mapper.Map(entityDto, entityToUpdate);

        await EntityRepository.UpdateAsync(entityToUpdate, cancellationToken: cancellationToken);

        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(id, nameof(id), ErrorMessages.CantBeNullOrEmpty);

        var entity = await EntityRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity);

        await EntityRepository.DeleteAsync(id, cancellationToken: cancellationToken);

        return BaseResponse.Success();
    }
}