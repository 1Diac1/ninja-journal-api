namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public interface IAggregateRoot<TKey> where TKey : struct
{
    public TKey Id { get; set; }
}