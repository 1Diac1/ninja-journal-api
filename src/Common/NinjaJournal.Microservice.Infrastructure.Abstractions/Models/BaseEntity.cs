namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public abstract class BaseEntity<TKey> : IAggregateRoot<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    
    protected BaseEntity() { }
    protected BaseEntity(TKey id) => Id = id;
}