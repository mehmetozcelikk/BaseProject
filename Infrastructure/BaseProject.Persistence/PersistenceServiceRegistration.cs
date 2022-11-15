﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Persistence.Concretes;
using BaseProject.Persistence.Contexts;
using BaseProject.Persistence.Repositories;
using BaseProject.Persistence.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Persistence;

public static class PersistenceServiceRegistration
{

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                        IConfiguration configuration)
    {
        //services.AddDbContext<BaseDbContext>(options =>
        //                                         options.UseSqlServer(
        //                                             configuration.GetConnectionString("SqlConnectionString")));
        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(
                                                     configuration.GetConnectionString("SqlConnectionString")));


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();

        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<IRefreshTokenService, RefreshTokenManager>();
        services.AddScoped<IOperationClaimService, OperationClaimManager>();
        services.AddScoped<IUserOperationClaimService, UserOperationManager>();
        services.AddScoped<IAuthService, AuthManager>();



        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        //services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        //services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        //services.AddScoped<IAuthService, AuthManager>();

        return services;


    }
}
