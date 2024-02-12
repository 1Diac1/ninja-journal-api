using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;

public abstract class BaseDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions<TDbContext> options)
        : base(options)
    { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}