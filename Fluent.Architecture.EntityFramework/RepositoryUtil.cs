using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fluent.Architecture.EntityFramework
{
    internal static class RepositoryUtil
    {
        internal static string GetForeignKeyFilterSql( object entity, Type outType, out bool nonKeys)
        {
            var tableName = outType.GetTableName();
            var fluentUniqueKeyValues = entity.GetForeignKeyValues(outType).Select(x => $"{x.ColumnName} = {x.Value}").ToArray();
            nonKeys = fluentUniqueKeyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", fluentUniqueKeyValues)})";// O and está no lugar certo sim
        }

        internal static string GetFluentUniqueKeyFilterSql(object entity, out bool nonKeys)
        {
            var tableName = entity.GetTableName();
            var fluentUniqueKeyValues = entity.GetFluentUniqueKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();
            nonKeys = fluentUniqueKeyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", fluentUniqueKeyValues)})";// O and está no lugar certo sim
        }

        internal static string GetKeyFilterSql(object entity, out bool nonKeys)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();
            nonKeys = keyValues.Length == 0;
            return $"select * from {tableName} where ({string.Join(" and ", keyValues)})"; // O and está no lugar certo sim
        }

        internal static string GetKeyAndFluentUniqueKeyFilterSql(object entity)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();
            var fluentUniqueKeyValues = entity.GetFluentUniqueKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();

            var sql = $"({string.Join(" and ", keyValues)})";// O and está no lugar certo sim

            if (fluentUniqueKeyValues.Length > 0)
            {
                sql += $" or ({string.Join(" or ", fluentUniqueKeyValues)})";
            }

            return $"select * from {tableName} where {sql}";
        }
    }
}
