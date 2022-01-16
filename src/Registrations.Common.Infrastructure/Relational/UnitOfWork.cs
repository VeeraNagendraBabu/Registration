using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Registrations.Common.Infrastructure.Relational.Interfaces;

namespace Registrations.Common.Infrastructure.Relational
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>, IAsyncDisposable
        where TDbContext : DbContext
    {
        private readonly bool _disposeDbContextOnDispose;
        private bool _disposed;
        public TDbContext DbContext { get; }
        private IServiceScope _serviceScope;
        private bool _isTransientScope;

        public UnitOfWork(TDbContext dbContext)
        {
            _disposeDbContextOnDispose = true;
            DbContext = dbContext;
        }

        ~UnitOfWork()         
        {             
            Cleanup();
            GC.SuppressFinalize(this);   
        }

        private void Cleanup()
        {
            if (_disposed || !_disposeDbContextOnDispose)
            {
                _disposed = true;
                return;
            }
            _disposed = true;
            DbContext.Dispose();
            DisposeWhenTransientScope();
        }

        

        public IDbConnection DbConnection => DbContext.Database.GetDbConnection();

        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            if (_disposed || !_disposeDbContextOnDispose)
            {
                _disposed = true;
                return default;
            }
            DisposeWhenTransientScope();
            _disposed = true;
            return DbContext.DisposeAsync();
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            await DbContext.Database.MigrateAsync(cancellationToken);
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void SetServiceScope(IServiceScope serviceScope)
        {
            _isTransientScope = true;
            _serviceScope = serviceScope;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
            => await DbContext.Database.BeginTransactionAsync(cancellationToken);

        private void DisposeWhenTransientScope()
        {
            if (_isTransientScope && _serviceScope != null)
            {
                _serviceScope.Dispose();
            }
        }
    }
}
