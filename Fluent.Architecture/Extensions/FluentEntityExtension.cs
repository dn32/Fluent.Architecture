// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    public static class FluentEntityExtension
    {
        public static string GetTypeName(this object entity)
        {
            return entity.GetType().Name;
        }

        public static string GetTableName(this object entity)
        {
            return entity.GetType().GetTableName();
        }

        public static string GetTableName(this Type entityType)
        {
            return entityType.GetCustomAttribute<TableAttribute>()?.Name ?? entityType.Name;
        }

        public static string GetColumnName(this PropertyInfo property)
        {
            return property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
        }

        public static string GetJsonPropertyName(this PropertyInfo property)
        {
            var name = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            if (string.IsNullOrEmpty(name))
            {
                name = property?.Name?.ToFluentJsonStringNormalized();
            }

        public static string GetUiPropertyName(this PropertyInfo property)
        {
            return property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.name ??
                   property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                   property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   property?.Name.ToFluentJsonStringNormalized();
        }

        public static PropertyInfo GetKeyProperty(this Type entityType)
        {
            return entityType
                 .GetProperties()
                 .Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttributeAny<KeyAttribute>(true))
                 .First();
        }

        public static int GetKeyValue(this object entity)
        {
            if (int.TryParse(entity.GetType().GetKeyProperty().GetValue(entity)?.ToString() ?? "NaN", out var id))
            {
                return id;
            }

            throw new InvalidOperationException();
        }

        public static List<PropertyInfo> GetKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x =>
                                                        x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) ||
                                                        x.GetCustomAttribute<KeyAttribute>(true) != null).ToList();
        }

        public static List<PropertyInfo> GetFluentUniqueKeyProperties(this Type entityType)
        {
            return entityType?.GetProperties()?.Where(x => x.GetCustomAttributeAny<FluentUniqueKeyAttribute>(true))?.ToList();
        }

        public static List<PropertyInfo> GetKeyAndFluentUniqueKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttributeAny<KeyAttribute>(true) || x.GetCustomAttributeAny<FluentUniqueKeyAttribute>(true)).ToList();
        }

        public static List<KeyValue> GetKeyAndFluentUniqueKeyValues(this object entity)
        {
            var properties = entity.GetType().GetKeyAndFluentUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        public static List<PropertyInfo> GetPropertiesByAttribute<TA>(this Type entityType) where TA : Attribute
        {
            return entityType.GetProperties().Where(x => x.GetCustomAttributeAny<TA>(true)).ToList();
        }

        public static T ChangeType<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static (int min, double max)? GetPropertyRange(this PropertyInfo property)
        {
            if (property.PropertyType.IsNumeric())
            {
                var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                if (min == null || max == null) { return null; }

                int minInt = min.ChangeType<int>();
                int maxInt = max.ChangeType<int>();

                return (maxInt, minInt);
            }

            if (property.PropertyType == typeof(string) && property.PropertyType == typeof(String))
            {
                var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                if (min == null || max == null) { return null; }

                return (min.Value, max.Value);
            }

            return null;
        }

        // Todo2 documentar
        public static List<KeyValue> GetFluentUniqueKeyValues(this object entity)
        {
            var properties = entity.GetType().GetFluentUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        public static List<KeyValue> GetForeignKeyValues(this object entity, Type outType)
        {
            var returnList = new List<KeyValue>();
            var localType = entity.GetType();
            var elements = entity
                        .GetType()
                        .GetProperties()
                        .Select(x => new { compositionAttr = x.GetCustomAttribute<FluentCompositionAttribute>(true), property = x })
                        .Where(x => x.compositionAttr != null).ToList();

            foreach (var element in elements)
            {
                var externalKeys = element.compositionAttr.ExternalKeys;
                var localKeys = element.compositionAttr.LocalKeys;
                var destinalType = element.property.PropertyType.IsList() ? element.property.PropertyType.GenericTypeArguments[0] : element.property.PropertyType;
                if (outType != destinalType) { continue; }

                for (int i = 0; i < externalKeys.Length; i++)
                {
                    var externalKey = externalKeys[i];
                    var localKey = localKeys[i];

                    var destinalKeyProperty = destinalType.GetProperty(externalKey);
                    if (destinalKeyProperty == null)
                    {
                        throw new IncorrectDevelopmentException($"Entity {entity.GetType().Name} has an incorrectly named foreign key because the reference property could not be found in entity {destinalType.Name}. The key in question has the name: '{externalKey}'.");
                    }

                    var localKeylProperty = localType.GetProperty(localKey);
                    if (localKeylProperty == null)
                    {
                        throw new IncorrectDevelopmentException($"Entity {localType.Name} has an incorrectly named foreign key because the reference property could not be found in entity {localType.Name}. The key in question has the name: '{localKey}'.");
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
            var properties = entity.GetType().GetKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        private static List<KeyValue> PropertiesToKeyValueList(object entity, List<PropertyInfo> properties)
        {
            return properties
                .Select(property =>
                        new KeyValue(
                            property,
                            property.GetDbValue(property.GetValue(entity)),
                            property.GetColumnName()
                        ))
                .ToList();
        }
    }
}
