using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Registrations.Common.Infrastructure.Relational.Interfaces;
using Registrations.Common.Infrastructure.Relational.Interfaces.Repositories;

namespace Registrations.Common.Infrastructure.Relational.Repositories
{
    public abstract class AsyncRepositoryBase<TEntity, TDbContext> : IAsyncRepositoryBase<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly IUnitOfWork<TDbContext> _uow;

        protected AsyncRepositoryBase(IUnitOfWork<TDbContext> uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _uow.DbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<(long Count, IReadOnlyList<TEntity> Entities)> GetPaginatedResponseAsync(int page, int size, CancellationToken cancellationToken = default)
        {
            var count = await _uow.DbContext.Set<TEntity>().CountAsync(cancellationToken);
            var data = await _uow.DbContext.Set<TEntity>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync(cancellationToken);
            return (count, data);
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await AddAsync(entity, false, cancellationToken);
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity, bool saveChanges = false,
            CancellationToken cancellationToken = default)
        {
            await _uow.DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            if (saveChanges)
            {
                await _uow.DbContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public virtual async ValueTask<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await UpdateAsync(entity, true, cancellationToken);
        }

        public async ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = false, CancellationToken cancellationToken = default)
        {
            _uow.DbContext.Set<TEntity>().UpdateRange(entities);

            if (saveChanges)
            {
                await _uow.DbContext.SaveChangesAsync(cancellationToken);
            }

            return default;
        }

        public virtual async ValueTask<int> UpdateAsync(TEntity entity,
            bool saveChanges = false, CancellationToken cancellationToken = default)
        {
            _uow.DbContext.Entry(entity).State = EntityState.Modified;
            if (saveChanges)
            {
                return await _uow.DbContext.SaveChangesAsync(cancellationToken);
            }
            return default;
        }

        public virtual async ValueTask<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await DeleteAsync(entity, true, cancellationToken);
        }

        public virtual async ValueTask<int> DeleteAsync(TEntity entity,
            bool saveChanges = false, CancellationToken cancellationToken = default)
        {
            _uow.DbContext.Set<TEntity>().Remove(entity);
            if (saveChanges)
            {
                return await _uow.DbContext.SaveChangesAsync(cancellationToken);
            }
            return default;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _uow.SaveChangesAsync(cancellationToken);

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default) =>
            _uow.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        protected DbSet<TEntity> GetDbSet() => GetDbSet(_uow);

        protected IQueryable<TEntity> GetQueryable() => GetDbSet(_uow);

        protected static DbSet<TEntity> GetDbSet(IUnitOfWork<TDbContext> uow) => uow.DbContext.Set<TEntity>();

        protected static IQueryable<TEntity> GetQueryable(IUnitOfWork<TDbContext> uow) => GetDbSet(uow);
    }
}