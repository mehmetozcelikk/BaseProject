using CorePackages.Security.EmailAuthenticator;
using CorePackages.Security.JWT;
using CorePackages.Security.OtpAuthenticator;
using Microsoft.Extensions.DependencyInjection;

namespace CorePackages.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        return services;
    }
}