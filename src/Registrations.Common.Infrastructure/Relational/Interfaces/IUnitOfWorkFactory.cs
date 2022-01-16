using Microsoft.EntityFrameworkCore;

namespace Registrations.Common.Infrastructure.Relational.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork<TDbContext> Create<TDbContext>()
            where TDbContext : DbContext;

        IUnitOfWork<TDbContext> Current<TDbContext>()
            where TDbContext : DbContext;
    }
}
