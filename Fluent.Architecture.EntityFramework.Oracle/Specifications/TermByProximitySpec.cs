using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public class TermByProximitySpec<TE> : FluentSpecification<TE> where TE : BaseEntity
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string ColumnName { get; set; }

        private int Tolerance { get; set; }

        public TermByProximitySpec<TE> AddParameter(string property, string term, int tolerance)
        {
            TableName = typeof(TE).GetTableName();
            ColumnName = typeof(TE).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Property not found {typeof(TE).Name}.{property}");
            Term = term;
            Tolerance = tolerance;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;

            var dbSet = query as DbSet<TE>;
            var sql = $@"
select * from {TableName}
where UTL_MATCH.jaro_winkler_similarity(lower({ColumnName}), lower({{0}})) > {Tolerance}
order by UTL_MATCH.jaro_winkler_similarity(lower({ColumnName}), lower({{0}})) DESC
";
            return dbSet.FromSql(sql, Term);
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
