using BaseProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;





namespace BaseProject.Persistence;

public static class ServiceRegistiration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services  )
    {

        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));

        services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));




        return services;
    }
}
