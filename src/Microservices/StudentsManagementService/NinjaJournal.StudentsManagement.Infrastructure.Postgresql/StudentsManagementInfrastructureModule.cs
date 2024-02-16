using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.StudentsManagement.Infrastructure.Postgresql;

public static class StudentsManagementInfrastructureModule
{
    public static void AddStudentsManagementInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StudentsManagementDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(nameof(StudentsManagementDbContext))));
    }
}