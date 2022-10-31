using BaseProject.Domain.Entities.Identity;
using BaseProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Persistence;

public static class ServiceRegistiration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                        IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options =>
                                                 options.UseSqlServer(
                                                     configuration.GetConnectionString("RentACarCampConnectionString")));
        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(""));

        services.AddIdentity<User, UserRole>();


        return services;
    }
}
