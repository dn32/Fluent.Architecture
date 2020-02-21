using dn32.infra.nucleo.excecoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Nucleo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using dn32.infra.nucleo.atributos;

namespace dn32.infra.extensoes
{
    public static class DnEntityExtension
    {
        // Todo2 documentar
        public static string GetTypeName(this object entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            return entity.GetType().Name;
        }

        // Todo2 documentar
        public static string GetTableName(this object entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            return entity.GetType().GetTableName();
        }

        // Todo2 documentar
        public static string GetTableName(this Type entityType)
        {
            if (entityType == null) { throw new ArgumentNullException(nameof(entityType)); }
            var name = entityType.GetCustomAttribute<TableAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = entityType.Name;
            }

            return name;
        }

        // Todo2 documentar
        public static string GetColumnName(this PropertyInfo property)
        {
            var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = property?.Name;
            }

            return name;
        }

        public static string GetJsonPropertyName(this PropertyInfo property)
        {
            var name = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            if (string.IsNullOrEmpty(name))
            {
                name = property?.Name?.ToDnJsonStringNormalized();
            }

            return name;
        }

        public static string GetUiPropertyName(this PropertyInfo property)
        {
            return property.GetCustomAttribute<DnPropriedadeJsonAtributo>()?.Nome ??
                   property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                   property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   property?.Name.ToDnJsonStringNormalized();
        }

        public static PropertyInfo GetKeyProperty(this Type entityType)
        {
            var properties = entityType?.GetProperties();
            return properties?.Length == 0 ? null : properties?.Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null)?.First();
        }

        public static int GetKeyValue(this object entity)
        {
            if (int.TryParse(entity?.GetType()?.GetKeyProperty()?.GetValue(entity)?.ToString(), out var id))
            {
                return id;
            }

            throw new InvalidOperationException();
        }

        public static List<PropertyInfo> GetKeyProperties(this Type entityType)
        {
            return entityType?.GetProperties()?.Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null)?.ToList();
        }

        public static List<PropertyInfo> GetDnUniqueKeyProperties(this Type entityType)
        {
            return entityType?.GetProperties()?.Where(x => x.IsDefined(typeof(DnChaveUnicaAtributo), true))?.ToList();
        }

        public static List<PropertyInfo> GetKeyAndDnUniqueKeyProperties(this Type entityType)
        {
            return entityType?.GetProperties()?.Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x?.GetCustomAttribute<KeyAttribute>(true) != null || x?.GetCustomAttribute<DnChaveUnicaAtributo>(true) != null)?.ToList();
        }

        public static List<KeyValue> GetKeyAndDnUniqueKeyValues(this object entity)
        {
            var properties = entity?.GetType()?.GetKeyAndDnUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        public static List<PropertyInfo> GetPropertiesByAttribute<TA>(this Type entityType) where TA : Attribute
        {
            return entityType?.GetProperties()?.Where(x => x.GetCustomAttribute<TA>(true) != null)?.ToList();
        }

        public static T ChangeType<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static (int min, double max)? GetPropertyRange(this PropertyInfo property)
        {
            if (property.PropertyType.IsNumeric())
            {
                var min = property.GetCustomAttribute<DnPropriedadeJsonAtributo>()?.Minimo ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                var max = property.GetCustomAttribute<DnPropriedadeJsonAtributo>()?.Maximo ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                if (min == null || max == null) { return null; }

                int minInt = min.ChangeType<int>();
                int maxInt = max.ChangeType<int>();

                return (maxInt, minInt);
            }

            if (property.PropertyType == typeof(string) && property.PropertyType == typeof(String))
            {
                var min = property.GetCustomAttribute<DnPropriedadeJsonAtributo>()?.Minimo ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                var max = property.GetCustomAttribute<DnPropriedadeJsonAtributo>()?.Maximo ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                if (min == null || max == null) { return null; }

                return (min.Value, max.Value);
            }

            return null;
        }

        // Todo2 documentar
        public static List<KeyValue> GetDnUniqueKeyValues(this object entity)
        {
            var properties = entity?.GetType()?.GetDnUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        public static List<KeyValue> GetForeignKeyValues(this object entity, Type outType)
        {
            static DnReferenciaAtributo GetReference(PropertyInfo property)
            {
                return property.GetCustomAttribute<DnComposicaoAtributo>(true) as DnReferenciaAtributo
                            ?? property.GetCustomAttribute<DnAgregacaoDeMuitosParaMuitosAtributo>(true) ?? null;
            }

            var returnList = new List<KeyValue>();
            var localType = entity.GetType();
            var elements = entity
                        .GetType()
                        .GetProperties()
                        .Select(x => new { compositionAttr = GetReference(x), property = x })
                        .Where(x => x.compositionAttr != null).ToList();

            foreach (var element in elements)
            {
                var externalKeys = element.compositionAttr.ChavesExternas;
                var localKeys = element.compositionAttr.ChavesLocais;
                var destinalType = element.property.PropertyType.IsList() ? element.property.PropertyType.GenericTypeArguments[0] : element.property.PropertyType;
                if (outType != destinalType) { continue; }

                for (int i = 0; i < externalKeys.Length; i++)
                {
                    var externalKey = externalKeys[i];
                    var localKey = localKeys[i];

                    var destinalKeyProperty = destinalType.GetProperty(externalKey);
                    if (destinalKeyProperty == null)
                    {
                        throw new DesenvolvimentoIncorretoException($"Entidade {entity.GetType().Name} has an incorrectly named foreign key because the Referencia property could not be found in entity {destinalType.Name}. The key in question has the Nome: '{externalKey}'.");
                    }

                    var localKeylProperty = localType.GetProperty(localKey);
                    if (localKeylProperty == null)
                    {
                        throw new DesenvolvimentoIncorretoException($"Entidade {localType.Name} has an incorrectly named foreign key because the Referencia property could not be found in entity {localType.Name}. The key in question has the Nome: '{localKey}'.");
                    }

                    var columnName = destinalKeyProperty.GetColumnName();
                    var value = localKeylProperty.GetValue(entity);

                    returnList.Add(new KeyValue
                    {
                        Property = localKeylProperty,
                        ColumnName = columnName,
                        Value = value.GetDbValue()
                    });
                }
            }

            return returnList;
        }

        // Todo2 documentar
        public static List<KeyValue> GetKeyValues(this object entity)
        {
            var properties = entity?.GetType()?.GetKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        private static List<KeyValue> PropertiesToKeyValueList(object entity, List<PropertyInfo> properties)
        {
            var returnList = new List<KeyValue>();

            foreach (var property in properties)
            {
                returnList.Add(new KeyValue
                {
                    Property = property,
                    ColumnName = property.GetColumnName(),
                    Value = property.GetValue(entity).GetDbValue(property)
                });
            }

            return returnList;
        }
    }
}
