// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
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
            return property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? property.Name;
        }

        public static string GetUiPropertyName(this PropertyInfo property)
        {
            return property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.name ??
                   property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                   property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   property.Name;
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
            if (int.TryParse(entity.GetType().GetKeyProperty().GetValue(entity).ToString(), out var id))
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
            return entityType.GetProperties().Where(x => x.GetCustomAttribute<FluentUniqueKeyAttribute>(true) != null).ToList();
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
