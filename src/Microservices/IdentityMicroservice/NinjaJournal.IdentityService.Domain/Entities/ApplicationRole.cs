using Microsoft.AspNetCore.Identity;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IAggregateRoot<Guid>, ICacheableEntity<Guid>
{ }