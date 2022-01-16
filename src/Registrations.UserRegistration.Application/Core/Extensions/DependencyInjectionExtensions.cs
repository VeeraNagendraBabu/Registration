using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registrations.Common.Infrastructure.Relational;
using Registrations.Common.Infrastructure.Relational.Extensions;
using Registrations.UserRegistration.Application.Core.Configurations;
using Registrations.UserRegistration.Application.Core.Handlers;
using Registrations.UserRegistration.Application.Core.Repositories;
using Registrations.UserRegistration.Application.Interfaces.Configurations;
using Registrations.UserRegistration.Application.Interfaces.Handlers;
using Registrations.UserRegistration.Application.Interfaces.Repositories;

namespace Registrations.UserRegistration.Application.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection UseRegistrationCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IConfig, EnvironmentConfig>();
            services.AddScoped<IUserHandler, UserHandler>();
            return services;
        }
        public static IServiceCollection UseConnectorDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScopedPersistence<RegistrationDbContext>(configuration, "RegistrationDbConnectionString");
            return services;
        }
    }
}
