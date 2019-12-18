using Fluent.Architecture.Extensions;
using Microsoft.EntityFrameworkCore;
using Fluent.Architecture.Core.Models;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public static class QueryExtension
    {
        public static IQueryable<T> WhereProximityText<T>(this IQueryable<T> query, string term, string table, string column, int tolerance) where T : FluentEntity
        {
            if (string.IsNullOrWhiteSpace(term)) { return query.OrderBy(x => x); }

            var sql = $@"
select * from {table}
where lower({column}) is not null and UTL_MATCH.jaro_winkler_similarity(lower({column}), lower({{0}})) > {tolerance}
order by UTL_MATCH.jaro_winkler_similarity(lower({column}), lower({{0}})) DESC
";

#if NETCOREAPP3_1
            var dbSet = query.FluentCast<DbSet<T>>();
            return dbSet.FromSqlRaw(sql, term);
#else
            return query.FromSql(sql, term);
#endif
        }

        public static IQueryable<T> WhereProximityText<T>(this IQueryable<T> query, string term, string table, string[] columns, int tolerance) where T : FluentEntity
        {
            if (string.IsNullOrWhiteSpace(term)) { return query.OrderBy(x => x); }

            var sqlArray = columns.Select(column => $@"
SELECT UTL_MATCH.JARO_WINKLER_SIMILARITY(LOWER({column}), LOWER({{0}})) PRECISAO, MXSPRODUT.* FROM {table} 
WHERE LOWER({column}) IS NOT NULL AND UTL_MATCH.JARO_WINKLER_SIMILARITY(LOWER({column}), LOWER({{0}})) >= {tolerance}");

            var union = string.Join("\nUNION ALL\n", sqlArray);
            var sql = $@"SELECT * FROM ({union}) ORDER BY PRECISAO DESC";

#if NETCOREAPP3_1
            var dbSet = query.FluentCast<DbSet<T>>();
            return dbSet.FromSqlRaw(sql, term);
#else
            return query.FromSql(sql, term);
#endif
        }
    }
}
