using BaseProject.Application.Mapping.UserOperationProfiles;
using BaseProject.Application.Mapping.UserProfiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseProject.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());


        services.AddScoped<UserProfiles>();
        services.AddScoped<UserOperationProfiles>();


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



        return services;

    }
}
