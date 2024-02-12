﻿namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

public abstract class BaseEntity<TKey>  
    where TKey : struct
{
    public TKey Id { get; protected set; }

    protected BaseEntity() { }

    protected BaseEntity(TKey id) => Id = id;
}