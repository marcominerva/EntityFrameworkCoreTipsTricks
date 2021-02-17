using Ef5TipsTricks.DataAccess.Entities;
using Ef5TipsTricks.DataAccess.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ef5TipsTricks.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var deletedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted
                && e.Entity.GetType().IsSubclassOf(typeof(DeletableEntity))).ToList();

            foreach (var entry in deletedEntries)
            {
                var entity = entry.Entity as DeletableEntity;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
