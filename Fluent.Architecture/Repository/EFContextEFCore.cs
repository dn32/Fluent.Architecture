//// ReSharper disable CommentTypo

#if NETCOREAPP2_1

using System;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public class EfContext : DbContext
    {
        internal string ConnectionString { get; set; }

        public EfContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Todas as entidades de banco de dados são adicionados automaticamente.
        /// Use <see cref="NotDbEntityAttribute"/> se não desejar que uma entidade seja adicionada.
        /// </summary>
        /// <param name="modelBuilder">
        /// O model builder do EF.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", Array.Empty<Type>());
            var exportedTypes = Setup.Model.Values.ToList();
            foreach (var type in exportedTypes)
            {
                if (type.GetCustomAttribute<NotDbEntityAttribute>(false) != null || type.IsAbstract)
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(FluentEntity)))
                {
                    if (entityMethod != null) entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, Array.Empty<object>());
                }
            }

            modelBuilder.Entity<Translation>().HasKey(c => new { c.Language, c.EntityType, c.EntityId, c.Property });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();
            changedEntities.ForEach(ChangedEvent);
            return base.SaveChanges();
        }

        private void ChangedEvent(EntityEntry entityChanged)
        {
            //var state = entityChanged.State;
            //var entity = entityChanged.Entity;
            var currentValues = entityChanged.CurrentValues;
            var originalValues = entityChanged.OriginalValues;

            var properties = originalValues.Properties.Select(x =>
                {
                    return new FluentEntityProperty
                    {
                        CurrentValue = currentValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) }).MakeGenericMethod(x.ClrType).Invoke(currentValues, new object[] { x }),
                        OriginalValue = originalValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) }).MakeGenericMethod(x.ClrType).Invoke(originalValues, new object[] { x }),
                        PropertyBane = x.Name
                    };
                }).ToList();


            //Disparar evento aqui!
        }
    }
}

#endif