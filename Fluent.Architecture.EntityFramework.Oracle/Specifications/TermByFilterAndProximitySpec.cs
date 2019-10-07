using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using System;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public class TermByFilterAndProximitySpec<TE> : FluentFilterSpec<TE> where TE : FluentEntity
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string ColumnName { get; set; }

        private int Tolerance { get; set; }

        public TermByFilterAndProximitySpec<TE> SetParameter(Filter[] filters, bool isList, string property, string term, int tolerance)
        {
            Filters = filters;
            IsList = isList;
            TableName = typeof(TE).GetTableName();
            Term = term;

            if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(Term))
            {
                ColumnName = typeof(TE).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Property not found '{typeof(TE).Name}.{property}'");
            }

            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;
            query = base.Where(query);

            if (!string.IsNullOrWhiteSpace(ColumnName) && !string.IsNullOrWhiteSpace(Term))
            {
                query = query.WhereProximityText(Term, TableName, ColumnName, Tolerance);
            }

            return query;
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
