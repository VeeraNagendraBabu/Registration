using Microsoft.EntityFrameworkCore;
using Registrations.Common.Domain.Entities;

namespace Registrations.Common.Infrastructure.Relational
{
    public class RegistrationDbContext : DbContextBase
    {
        public RegistrationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            RegistrationConfiguration(modelBuilder);
        }
        private static void RegistrationConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }

    }
}
