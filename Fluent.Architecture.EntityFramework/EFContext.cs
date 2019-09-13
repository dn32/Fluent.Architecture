// ReSharper disable CommentTypo
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public abstract class EfContext : DbContext
    {
        internal delegate void EntityChangeEventHandler(FluentEventEntity fluentEventEntity);
        internal event EntityChangeEventHandler EntityChangingEventEvent;
        internal event EntityChangeEventHandler EntityChangedEventEvent;

        protected internal string ConnectionString { get; set; }

        public EfContext(string connectionString)
        {
            ConnectionString = connectionString;
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
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
            var exportedTypes = Setup.Model.Values.ToList();
            foreach (var type in exportedTypes)
            {
                if (type.GetCustomAttribute<NotDbEntityAttribute>(false) != null || type.IsAbstract)
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(FluentEntity)))
                {
                    var keys = type.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() != null).Select(x => x.Name).ToArray();
                    if (keys.Length == 0)
                    {
                        throw new Exception($"The entity {type.Name} must contains a least one key");
                    }

                    var entity = modelBuilder.Entity(type);
                    entity.HasKey(keys);

                    if (UseLogicalDeletion)
                    {
                        entity.AddQueryFilter(IsAvailable());
                    }

                    SetEntity(entity, type);

                    var navigations = entity.Metadata.GetNavigations();
                    foreach (var property in type.GetProperties())
                    {
                        if (navigations.Any(x => x.Name == property.Name))
                        {
                            continue;
                        }

                        if (property.PropertyType.IsNullableEnum())
                        {
                            if (property.PropertyType.GetCustomAttribute<FluentUseEnumValueToDBAttribute>() == null)
                            {
                                entity.Property(property.Name).HasConversion<string>();// Converte os enumeradores para salvar o valor string no BD
                            }
                        }

                        if (property.GetCustomAttribute<NotMappedAttribute>() != null)
                        {
                            entity.Ignore(property.Name);
                            continue;
                        }

                        SetEntityProperty(entity, type, property);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected virtual void SetEntityProperty(EntityTypeBuilder entity, Type type, PropertyInfo property) { }

        protected virtual void SetEntity(EntityTypeBuilder entity, Type type) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        public override int SaveChanges()
        {
            UpdateLogicalDeletion(ChangeTracker.Entries());

            BeforeSave(out var changedEntities, out var eventChange);

            var ret = base.SaveChanges();

            AfterSave(changedEntities, eventChange);

            return ret;
        }

        protected virtual void UpdateLogicalDeletion(IEnumerable<EntityEntry> entries) { }

        protected virtual LambdaExpression IsAvailable()
        {
            throw new IncorrectDevelopmentException($"The Enable {nameof(UseLogicalDeletion)} property set to 'true' requires the override of the {nameof(IsAvailable)} method in the context of the entity framework. Do not invoke the base.");
        }

        protected virtual bool UseLogicalDeletion => false;

        public virtual bool EnableLogicalDeletion { get; set; }

        private void AfterSave(List<EntityEntry> changedEntities, List<FluentEventEntity> eventChange)
        {
            if (EntityChangedEventEvent == null)
            {
                return;
            }

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

            if (EntityChangingEventEvent == null)
            {
                return;
            }

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
