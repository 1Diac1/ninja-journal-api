using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.StudentsManagement.Infrastructure.Postgresql;

public class StudentsManagementDbContext : BaseDbContext<StudentsManagementDbContext>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }

    public StudentsManagementDbContext(DbContextOptions<StudentsManagementDbContext> options) 
        : base(options)
    { }
}