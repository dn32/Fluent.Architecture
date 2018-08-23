using Fluent.Architecture.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    public static class FluentEntityExtension
    {
        //Todo documentar
        public static string GetTableName(this object entity)
        {
            return entity.GetType().GetTableName();
        }

        //Todo documentar
        public static string GetTableName(this Type entityType)
        {
            var name = entityType.GetCustomAttribute<TableAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = entityType.Name;
            }

            return name;
        }

        //Todo documentar
        public static string GetColumnName(this PropertyInfo property)
        {
            var name = property.GetCustomAttribute<ColumnAttribute>()?.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = property.Name;
            }

            return name;
        }

        public static List<PropertyInfo> GetKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || x.GetCustomAttribute<KeyAttribute>(true) != null).ToList();
        }

        public static List<PropertyInfo> GetFluentUnicKeyProperties(this Type entityType)
        {
            return entityType.GetProperties().Where(x => x.GetCustomAttribute<FluentUnicKeyAttribute>(true) != null).ToList();
        }

        public static List<PropertyInfo> GetPropertiesByAttribute<TA>(this Type entityType) where TA : Attribute
        {
            return entityType.GetProperties().Where(x => x.GetCustomAttribute<TA>(true) != null).ToList();
        }

        //Todo documentar
        public static List<KeyValuePair<string, object>> GetKeyValues(this object entity)
        {
            var returnList = new List<KeyValuePair<string, object>>();
            var properties = entity.GetType().GetKeyProperties();
            foreach (var property in properties)
            {
                //Todo resolver problema com enumeradores aqui
                var key = property.GetColumnName();
                returnList.Add(new KeyValuePair<string, object>(key, property.GetValue(entity).GetDbValue()));
            }

            return returnList;
        }
    }
}
