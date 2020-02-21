using dn32.infra.extensoes;
using dn32.infra.Nucleo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dn32.infra.EntityFramework
{
    internal static class RepositoryUtil
    {
        internal static string ListToInSql(Type dbEntityType, object[] elements, PropertyInfo property)
        {
            if (elements is null || elements.Length == 0)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            var tableName = dbEntityType.GetTableName();
            var collumnName = property.GetColumnName();

            var elementsList = new List<string>();
            foreach (var element in elements)
            {
                var value = element.GetDbValue().ToString();
                elementsList.Add(value);
            }

            var elementsStirng = string.Join(",", elementsList.ToArray());
            var sql = $"select {collumnName} from {tableName} where {collumnName} in ({elementsStirng})";
            return sql;
        }

        private static string GetStringOrNumberValue(KeyValue keyValue)
        {
            return $"{keyValue.ColumnName} = {keyValue.Value}";
        }

        internal static string GetForeignKeyFilterSql(object entity, Type outType, out bool nonKeys)
        {
            var tableName = outType.GetTableName();
            var fluentUniqueKeyValues = entity.GetForeignKeyValues(outType).Select(GetStringOrNumberValue).ToArray();
            nonKeys = fluentUniqueKeyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", fluentUniqueKeyValues)})";// O and está no lugar certo sim
        }

        internal static string GetDnUniqueKeyFilterSql(object entity, out bool nonKeys)
        {
            var tableName = entity.GetTableName();
            var fluentUniqueKeyValues = entity.GetDnUniqueKeyValues().Select(GetStringOrNumberValue).ToArray();
            nonKeys = fluentUniqueKeyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", fluentUniqueKeyValues)})";// O and está no lugar certo sim
        }

        internal static string GetKeyFilterSql(object entity, out bool nonKeys)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(GetStringOrNumberValue).ToArray();
            nonKeys = keyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", keyValues)})"; // O and está no lugar certo sim
        }

        internal static string GetKeyAndDnUniqueKeyFilterSql(object entity)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(GetStringOrNumberValue).ToArray();
            var fluentUniqueKeyValues = entity.GetDnUniqueKeyValues().Select(GetStringOrNumberValue).ToArray();

            var sql = $"({string.Join(" and ", keyValues)})";// O and está no lugar certo sim

            if (fluentUniqueKeyValues.Length > 0)
            {
                sql += $" or ({string.Join(" or ", fluentUniqueKeyValues)})";
            }

            return $"select * from {tableName} where {sql}";
        }
    }
}
