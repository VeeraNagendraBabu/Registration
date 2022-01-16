using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Registrations.Common.Infrastructure.Relational.Interfaces;

namespace Registrations.Common.Infrastructure.Relational
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork<TDbContext> Create<TDbContext>()
            where TDbContext : DbContext
        {
            var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetService<IUnitOfWork<TDbContext>>();
            uow.SetServiceScope(scope);
            return uow;
        }

        public IUnitOfWork<TDbContext> Current<TDbContext>()
            where TDbContext : DbContext
        {
            return _serviceProvider.GetService<IUnitOfWork<TDbContext>>();
        }
    }
}
