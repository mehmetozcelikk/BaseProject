using BaseProject.Application.Mapping.UserOperationProfiles;
using BaseProject.Application.Mapping.UserProfiles;
using CorePackages.CrossCuttingConcerns.Logging.Serilog.Logger;
using CorePackages.CrossCuttingConcerns.Logging.Serilog;
using CorePackages.Mailing;
using CorePackages.Mailing.MailKitImplementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BaseProject.Application.Abstractions.Services;

namespace BaseProject.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());


        services.AddScoped<UserProfiles>();
        services.AddScoped<UserOperationProfiles>();


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        services.AddSingleton<LoggerServiceBase, FileLogger>();


        return services;

    }
}
