using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registrations.Common.Infrastructure.Relational.Interfaces;
using Registrations.Common.Infrastructure.Relational.Interfaces.Repositories;
using Registrations.Common.Infrastructure.Relational.Repositories;

namespace Registrations.Common.Infrastructure.Relational.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const int CommandTimeout = 5;
        
        public static void AddScopedPersistence<TDbContext>(this IServiceCollection services,
          IConfiguration configuration, string connectionStringName)
          where TDbContext : DbContext
        {
            AddScopedPersistence<TDbContext>(services, configuration, connectionStringName, string.Empty);
        }

        public static void AddScopedPersistence<TDbContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName, string migrationsAssembly)
            where TDbContext : DbContext
        {
            AddPersistence<TDbContext>(services, configuration, connectionStringName, migrationsAssembly, ServiceLifetime.Scoped);
            services.AddScoped(typeof(IAsyncRepository<,,>), typeof(AsyncRepository<,,>));
        }

        public static void AddPersistence<TDbContext>(this IServiceCollection services, IConfiguration configuration,
            string connectionStringName, string migrationsAssembly, ServiceLifetime contextLifetime) where TDbContext : DbContext
        {
            services.AddLifetimeUow<TDbContext>(contextLifetime);
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddLifetimeDbContext<TDbContext>(configuration, connectionStringName, migrationsAssembly, contextLifetime);
        }

        private static void AddLifetimeDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration,
            string connectionStringName, string migrationsAssembly, ServiceLifetime contextLifetime = ServiceLifetime.Transient)
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetValue<string>(connectionStringName) ??
                                   configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionStringName), "connectionString is null");
            }

            services.AddDbContext<TDbContext>(options =>
            {
                options.UseSqlite(connectionString, options =>
                {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                }); 

            }, contextLifetime);
        }

        private static void AddLifetimeUow<TDbContext>(this IServiceCollection services, ServiceLifetime contextLifetime)
            where TDbContext : DbContext
        {
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IUnitOfWork<TDbContext>), typeof(UnitOfWork<TDbContext>));
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IUnitOfWork<TDbContext>), typeof(UnitOfWork<TDbContext>));
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IUnitOfWork<TDbContext>), typeof(UnitOfWork<TDbContext>));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contextLifetime), contextLifetime, null);
            }
        }

       
    }
}
