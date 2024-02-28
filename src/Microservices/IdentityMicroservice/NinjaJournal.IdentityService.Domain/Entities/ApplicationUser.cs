using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAggregateRoot<Guid>
{
    public Guid StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; init; }
}