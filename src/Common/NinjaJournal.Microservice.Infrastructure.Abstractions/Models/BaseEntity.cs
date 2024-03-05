namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public abstract class BaseEntity<TKey> : IAggregateRoot<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }
    
    protected BaseEntity() { }
    protected BaseEntity(TKey id) => Id = id;
}