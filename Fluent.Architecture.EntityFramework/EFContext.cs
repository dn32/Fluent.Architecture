using dn32.infra.atributos;
using dn32.infra.dados;
using dn32.infra.enumeradores;
using dn32.infra.extensoes;
using dn32.infra.Interfaces;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using dn32.infra.nucleo.excecoes;

namespace dn32.infra.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public abstract class EfContext : DbContext
    {
        internal protected delegate void EntityChangeEventHandler(ICollection<DnEventEntity> fluentEventEntity);
        internal protected event EntityChangeEventHandler EntityChangingEventEvent;
        internal protected event EntityChangeEventHandler EntityChangedEventEvent;

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
        /// <param Nome="modelBuilder">
        /// O model builder do EF.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var exportedTypes = Setup.Modelos.Values.ToList();
            foreach (var type in exportedTypes)
            {
                if (type.IsDefined(typeof(NotMappedAttribute), false) || type.IsAbstract)
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(DnEntidade)))
                {
                    var keys = type.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() != null).Select(x => x.Name).ToArray();
                    if (keys.Length == 0)
                    {
                        throw new Exception($"The entity {type.Name} must contains a least one key");
                    }

                    var entity = modelBuilder.Entity(type);
                    entity.HasKey(keys);

                    if (UseLogicalDeletion(type, entity, modelBuilder))
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

                        if (property.IsDefined(typeof(NotMappedAttribute), true))
                        {
                            entity.Ignore(property.Name);
                            continue;
                        }

                        if (property.PropertyType.IsNullableEnum())
                        {
                            if (property.PropertyType.IsNullable())
                            {
                                // Converte valores de enumeradores para null quando necessário
                                var method = GetType().GetMethod(nameof(ConvertNulableEnum), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(property.PropertyType.GetNonNullableType());
                                method?.Invoke(this, new object[] { entity, property });
                            }
                            else
                            {
                                if (!property.PropertyType.GetListTypeNonNull().IsDefined(typeof(DnUsarStringParaEnumeradoresNoBdAtributo)))
                                {
                                    entity.Property(property.Name).HasConversion<string>();// Converte os enumeradores para salvar o valor string no BD
                                }
                            }
                        }

                        SetEntityProperty(entity, type, property);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected void ConvertNulableEnum<TEnum>(EntityTypeBuilder entity, PropertyInfo property) where TEnum : Enum
        {
            if (typeof(TEnum).GetCustomAttribute<DnValorNuloParaEnumeradorAtributo>() is DnValorNuloParaEnumeradorAtributo fluentEnumValueForSetNullAttribute)
            {
                ValueConverter converter = null;

                if (property.PropertyType.GetListTypeNonNull().IsDefined(typeof(DnUsarStringParaEnumeradoresNoBdAtributo)))
                {
                    converter = new ValueConverter<TEnum, int?>(v => v.GetHashCode() == fluentEnumValueForSetNullAttribute.Valor ? null : (int?)v.GetHashCode(), v => (TEnum)Enum.ToObject(typeof(TEnum), v ?? 0));
                }
                else
                {
                    converter = new ValueConverter<TEnum, string>(v => v.GetHashCode() == fluentEnumValueForSetNullAttribute.Valor ? null : v.ToString(), v => (TEnum)Enum.Parse(typeof(TEnum), v));
                }

                entity.Property(property.Name).HasConversion(converter);
            }
            else
            {
                if (!property.PropertyType.GetListTypeNonNull().IsDefined(typeof(DnUsarStringParaEnumeradoresNoBdAtributo)))
                {
                    entity.Property(property.Name).HasConversion<string>();// Converte os enumeradores para salvar o valor string no BD
                }
            }
        }

        protected virtual void SetEntityProperty(EntityTypeBuilder entity, Type type, PropertyInfo property) { }

        protected virtual void SetEntity(EntityTypeBuilder entity, Type type) { }

        protected static LoggerFactory ContextLogFactory = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG

            ContextLogFactory ??= new LoggerFactory(new[] { new DebugLoggerProvider() });
            optionsBuilder
                .UseLoggerFactory(ContextLogFactory)
                    .EnableSensitiveDataLogging();

#endif
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateLogicalDeletion(ChangeTracker.Entries());

            var eventChange = BeforeSave();

            var ret = await base.SaveChangesAsync();

            AfterSave(eventChange);

            return ret;
        }

        protected virtual void UpdateLogicalDeletion(IEnumerable<EntityEntry> entries)
        {
        }

        protected virtual LambdaExpression IsAvailable()
        {
            throw new DesenvolvimentoIncorretoException($"The Enable {nameof(UseLogicalDeletion)} method set to 'true' requires the override of the {nameof(IsAvailable)} method in the context of the entity framework. Do not invoke the base.");
        }

        protected virtual bool UseLogicalDeletion(Type type, EntityTypeBuilder entity, ModelBuilder modelBuilder) => false;

        public virtual bool EnableLogicalDeletion { get; set; }

        internal protected SessaoDeRequisicaoDoUsuario UserSessionRequest { get; internal set; }

        private void AfterSave(List<DnEventEntity> eventChangeList)
        {
            if (EntityChangedEventEvent == null) { return; }
            eventChangeList.ForEach(x => SetEventChangeCurrentValue(x));
            EntityChangedEventEvent.Invoke(eventChangeList);
        }

        private List<DnEventEntity> BeforeSave()
        {
            var changedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();
            var eventChangeList = changedEntities.Select(GetEventChange).Where(x => x != null).ToList();
            EntityChangingEventEvent?.Invoke(eventChangeList);
            return eventChangeList;
        }

        private void SetEventChangeCurrentValue(DnEventEntity fluentEventEntity)
        {
            var currentValuesGetValue = fluentEventEntity.ChangedEntity.CurrentValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });
            var properties = fluentEventEntity.ChangedEntity.CurrentValues.Properties.ToList();

            fluentEventEntity.Properties.ForEach(x =>
            {
                var property = properties.Next();
                x.CurrentValue = currentValuesGetValue?.MakeGenericMethod(property.ClrType).Invoke(fluentEventEntity.ChangedEntity.CurrentValues, new[] { property });
            });
        }

        private DnEventEntity GetEventChange(EntityEntry entityChanged)
        {
            var currentValuesGetValue = entityChanged.CurrentValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });
            var originalValuesGetValue = entityChanged.OriginalValues.GetType().GetMethod("GetValue", new[] { typeof(IProperty) });

            var properties = entityChanged.OriginalValues.Properties.Select(x =>
            {
                return new DnEventEntityProperty
                {
                    CurrentValue = currentValuesGetValue?.MakeGenericMethod(x.ClrType).Invoke(entityChanged.CurrentValues, new[] { x }),
                    OriginalValue = originalValuesGetValue?.MakeGenericMethod(x.ClrType).Invoke(entityChanged.OriginalValues, new[] { x }),
                    PropertyName = x.Name
                };
            }).ToList();

            var currentEntityType = entityChanged.Entity.GetType();

            if (currentEntityType.GetCustomAttribute<DnLogAtributo>()?.Apresentar == EnumApresentar.Ocultar)
            {
                return null;
            }

            return new DnEventEntity
            {
                Properties = properties,
                CurrentEntity = entityChanged.Entity,
                CurrentEntityType = currentEntityType,
                ChangedEntity = entityChanged,
                EntityState = entityChanged.State
            };
        }
    }
}
