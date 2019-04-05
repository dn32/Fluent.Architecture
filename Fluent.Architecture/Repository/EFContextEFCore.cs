//// ReSharper disable CommentTypo

#if NETCOREAPP2_1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Extensions;
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
        internal delegate void EntityChangeEventHandler(FluentEventEntity fluentEventEntity);
        internal event EntityChangeEventHandler EntityChangingEventEvent;
        internal event EntityChangeEventHandler EntityChangedEventEvent;

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
            BeforeSave(out var changedEntities, out var eventChange);

            var ret = base.SaveChanges();

            AfterSave(changedEntities, eventChange);

            return ret;
        }

        private void AfterSave(List<EntityEntry> changedEntities, List<FluentEventEntity> eventChange)
        {
            changedEntities.ForEach(x =>
            {
                var fluentEventEntity = eventChange.Next();
                SetEventChangeCurrentValue(x, fluentEventEntity);
                EntityChangedEventEvent(fluentEventEntity);
            });
        }

        private void BeforeSave(out List<EntityEntry> changedEntities, out List<FluentEventEntity> eventChange)
        {
            changedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();
            eventChange = changedEntities.Select(GetEventChange).ToList();
            eventChange.ForEach(x => EntityChangingEventEvent(x));
        }

        private void SetEventChangeCurrentValue(EntityEntry entityChanged, FluentEventEntity fluentEventEntity)
        {
            var currentValuesGetValue = entityChanged.CurrentValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });
            var properties = entityChanged.CurrentValues.Properties.ToList();

            fluentEventEntity.Properties.ForEach(x =>
            {
                var property = properties.Next();
                x.CurrentValue = currentValuesGetValue.MakeGenericMethod(property.ClrType).Invoke(entityChanged.CurrentValues, new[] { property });
            });
        }

        private FluentEventEntity GetEventChange(EntityEntry entityChanged)
        {
            var currentValuesGetValue = entityChanged.CurrentValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });
            var originalValuesGetValue = entityChanged.OriginalValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });

            var properties = entityChanged.OriginalValues.Properties.Select(x =>
            {
                return new FluentEventEntityProperty
                {
                    CurrentValue = currentValuesGetValue.MakeGenericMethod(x.ClrType).Invoke(entityChanged.CurrentValues, new[] { x }),
                    OriginalValue = originalValuesGetValue.MakeGenericMethod(x.ClrType).Invoke(entityChanged.OriginalValues, new[] { x }),
                    PropertyBane = x.Name
                };
            }).ToList();

            return new FluentEventEntity
            {
                Properties = properties,
                CurrentEntity = entityChanged.Entity
            };
        }
    }
}

#endif