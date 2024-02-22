namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public abstract class BaseEntityDto<TKey>
    where TKey : struct
{
    public TKey Id { get; set; }
}