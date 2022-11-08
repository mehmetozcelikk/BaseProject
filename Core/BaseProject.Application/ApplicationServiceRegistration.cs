﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Mapping.UserProfiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());


        services.AddScoped<UserProfiles>();


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



        return services;

    }
}
