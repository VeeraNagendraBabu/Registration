using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Registrations.Common.Domain.Entities;

namespace Registrations.Common.Infrastructure.Relational.Interfaces.Repositories
{
    public interface IAsyncRepository<TEntity, TDbContext, in TPk> : IAsyncRepositoryBase<TEntity> 
        where TEntity : class, IEntity<TPk> 
        where TDbContext : DbContext
        where TPk : IComparable, IConvertible, IEquatable<TPk>
    {
         Task<TEntity> FindAsync(TPk id);

         Task<IReadOnlyList<TEntity>> GetByIds(params TPk[] ids);

         Task<IList<TEntity>> GetByIdsTracking(params TPk[] ids);
    }
}
