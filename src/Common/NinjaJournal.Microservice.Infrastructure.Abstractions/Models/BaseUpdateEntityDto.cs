namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public class BaseUpdateEntityDto<TKey> : BaseEntityDto<TKey>
    where TKey : struct
{ }