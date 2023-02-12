using Microsoft.EntityFrameworkCore;
using ubank_api.Data.Models;
using ubank_api.Data.Models.Entities;

namespace ubank_api.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<City> Cities => Set<City>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Client> Clients => Set<Client>();

        public DbSet<Account> Accounts => Set<Account>();

        public DbSet<Transaction> Transactions => Set<Transaction>();

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).DateModified = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).DateCreated = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
