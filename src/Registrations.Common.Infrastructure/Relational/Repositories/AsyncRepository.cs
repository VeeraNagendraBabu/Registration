using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Registrations.Common.Domain.Entities;
using Registrations.Common.Infrastructure.Relational.Interfaces;
using Registrations.Common.Infrastructure.Relational.Interfaces.Repositories;

namespace Registrations.Common.Infrastructure.Relational.Repositories
{
    public class AsyncRepository<TEntity, TDbContext, TPk> : AsyncRepositoryBase<TEntity, TDbContext>, 
        IAsyncRepository<TEntity, TDbContext, TPk> 
            where TEntity : class, IEntity<TPk> 
            where TDbContext : DbContext
            where TPk : IComparable, IConvertible, IEquatable<TPk>
    {
        public AsyncRepository(IUnitOfWork<TDbContext> uow) : base(uow)
        {
        }

        public virtual async Task<TEntity> FindAsync(TPk id)
        {
            return await GetDbSet().FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> GetByIds(params TPk[] ids)
        {
            return await GetDbSet().Where(entity=> ids.Contains(entity.Id)).ToArrayAsync();
        }

        public async Task<IList<TEntity>> GetByIdsTracking(params TPk[] ids)
        {
            return await GetDbSet().AsTracking().Where(entity=> ids.Contains(entity.Id)).ToListAsync();
        }
    }
}
