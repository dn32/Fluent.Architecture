using Fluent.Architecture.Extensions;
using Microsoft.EntityFrameworkCore;
using Fluent.Architecture.Core.Models;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public static class QueryExtension
    {
        public static IQueryable<T> WhereProximityText<T>(this IQueryable<T> query, string term, string table, string collumn, int tolerance) where T : FluentEntity
        {
            if (string.IsNullOrWhiteSpace(term)) { return query.OrderBy(x => x); }

            var sql = $@"
select * from {table}
where lower({collumn}) is not null and UTL_MATCH.jaro_winkler_similarity(lower({collumn}), lower({{0}})) > {tolerance}
order by UTL_MATCH.jaro_winkler_similarity(lower({collumn}), lower({{0}})) DESC
";

#if NETCOREAPP3_0
            var dbSet = query.FluentCast<DbSet<T>>();
            return dbSet.FromSqlRaw(sql, term);
#else
            return query.FromSql(sql, term);
#endif
        }
    }
}
