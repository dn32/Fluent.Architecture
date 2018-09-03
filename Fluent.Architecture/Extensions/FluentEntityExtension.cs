// ReSharper disable CommentTypo
using Fluent.Architecture.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Extensions
{
    public static class FluentEntityExtension
    {
        // Todo documentar
        public static string GetTypeName(this object entity)
        {
            return entity.GetType().Name;
        }

        // Todo documentar
        public static string GetTableName(this object entity)
        {
            return entity.GetType().GetTableName();
        }

        // Todo documentar
        public static string GetTableName(this Type entityType)
        {
            var name = entityType.GetCustomAttribute<TableAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = entityType.Name;
            }

            return name;
        }

        // Todo documentar
        public static string GetColumnName(this PropertyInfo property)
        {
            var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = property.Name;
            }

            return name;
        }

        public static PropertyInfo GetKeyProperty(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null)?.First();
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
            return entityType.GetProperties().Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null).ToList();
        }

        public static List<PropertyInfo> GetFluentUniqueKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.GetCustomAttribute<FluentUniqueKeyAttribute>(true) != null).ToList();
        }

        public static List<PropertyInfo> GetKeyAndFluentUniqueKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null || x.GetCustomAttribute<FluentUniqueKeyAttribute>(true) != null).ToList();
        }

        public static List<KeyValue> GetKeyAndFluentUniqueKeyValues(this object entity)
        {
            var properties = entity.GetType().GetKeyAndFluentUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        public static List<PropertyInfo> GetPropertiesByAttribute<TA>(this Type entityType) where TA : Attribute
        {
            return entityType.GetProperties().Where(x => x.GetCustomAttribute<TA>(true) != null).ToList();
        }

        // Todo documentar
        public static List<KeyValue> GetFluentUniqueKeyValues(this object entity)
        {
            var properties = entity.GetType().GetFluentUniqueKeyProperties();
            return PropertiesToKeyValueList(entity, properties);
        }

        // Todo documentar
        public static List<KeyValue> GetKeyValues(this object entity)
        {
            var properties = entity.GetType().GetKeyProperties();
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
