using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Registrations.Common.Infrastructure.Relational.Interfaces.Repositories
{
    public interface IAsyncRepositoryBase<TEntity>
        where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<(long Count, IReadOnlyList<TEntity> Entities)> GetPaginatedResponseAsync(int page, int size, CancellationToken cancellationToken = default);

        ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        ValueTask<TEntity> AddAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default);

        ValueTask<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entity, bool saveChanges = false, CancellationToken cancellationToken = default);

        ValueTask<int> UpdateAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default);

        ValueTask<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        ValueTask<int> DeleteAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
