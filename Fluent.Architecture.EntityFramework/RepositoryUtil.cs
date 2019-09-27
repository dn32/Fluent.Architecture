using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using System;
using System.Linq;

namespace Fluent.Architecture.EntityFramework
{
    internal static class RepositoryUtil
    {
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

        internal static string GetFluentUniqueKeyFilterSql(object entity, out bool nonKeys)
        {
            var tableName = entity.GetTableName();
            var fluentUniqueKeyValues = entity.GetFluentUniqueKeyValues().Select(GetStringOrNumberValue).ToArray();
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

        internal static string GetKeyAndFluentUniqueKeyFilterSql(object entity)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(GetStringOrNumberValue).ToArray();
            var fluentUniqueKeyValues = entity.GetFluentUniqueKeyValues().Select(GetStringOrNumberValue).ToArray();

            var sql = $"({string.Join(" and ", keyValues)})";// O and está no lugar certo sim

            if (fluentUniqueKeyValues.Length > 0)
            {
                sql += $" or ({string.Join(" or ", fluentUniqueKeyValues)})";
            }

            return $"select * from {tableName} where {sql}";
        }
    }
}
