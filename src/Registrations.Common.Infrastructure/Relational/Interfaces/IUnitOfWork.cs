using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Registrations.Common.Infrastructure.Relational.Interfaces
{
    public interface IUnitOfWork<out TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        TDbContext DbContext { get; }

        Task MigrateAsync(CancellationToken cancellationToken);

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        void SetServiceScope(IServiceScope serviceScope);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
