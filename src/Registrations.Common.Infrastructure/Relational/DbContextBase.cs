using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Registrations.Common.Infrastructure.Relational
{
    [ExcludeFromCodeCoverage]
    public class DbContextBase : DbContext
    {
        protected DbContextBase()
        {
        }

        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}