using Fluent.Architecture.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public static class QueryExtension
    {
        public static IQueryable<T> WhereProximityText<T>(this IQueryable<T> query, string term, string table, string collumn, int tolerance) where T : FluentEntity
        {
            if (string.IsNullOrWhiteSpace(term)) { return query.OrderBy(x => x); }

            var dbSet = query as DbSet<T>;
            var sql = $@"
select * from {table}
where UTL_MATCH.jaro_winkler_similarity(lower({collumn}), lower({{0}})) > {tolerance}
order by UTL_MATCH.jaro_winkler_similarity(lower({collumn}), lower({{0}})) DESC
";

#if NETCOREAPP3_0
            return dbSet.FromSqlRaw(sql, term);
#else
            return dbSet.FromSql(sql, term);
#endif
        }
    }
}
