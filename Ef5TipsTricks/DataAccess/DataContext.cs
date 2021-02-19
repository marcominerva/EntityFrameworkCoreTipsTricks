using Ef5TipsTricks.DataAccess.Entities;
using Ef5TipsTricks.DataAccess.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ef5TipsTricks.DataAccess
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public IQueryable<T> Get<T>(bool trackingChanges = false) where T : class
        {
            var set = Set<T>();
            return trackingChanges ? set : set.AsNoTracking();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var trimStringConverter = new ValueConverter<string, string>(v => v.Trim(), v => v.Trim());
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion(trimStringConverter);
                    }
                }
            }

            var entities = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(DeletableEntity).IsAssignableFrom(t.ClrType)).ToList();

            foreach (var type in entities.Select(t => t.ClrType))
            {
                var methods = SetGlobalQueryMethods(type);
                foreach (var method in methods)
                {
                    var generic = method.MakeGenericMethod(type);
                    generic.Invoke(this, new object[] { modelBuilder });
                }
            }

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

        private void SetQueryFilterOnDeletableEntity<T>(ModelBuilder builder) where T : DeletableEntity
            => builder.Entity<T>().HasQueryFilter(t => !t.IsDeleted);

        private static readonly MethodInfo setQueryFilterOnDeletableEntity = typeof(DataContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilterOnDeletableEntity));

        private static IEnumerable<MethodInfo> SetGlobalQueryMethods(Type type)
        {
            var result = new List<MethodInfo>();

            if (typeof(DeletableEntity).IsAssignableFrom(type))
            {
                result.Add(setQueryFilterOnDeletableEntity);
            }

            return result;
        }
    }
}
