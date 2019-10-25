using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using System;
using System.Linq;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Core.Extensions;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public class TermByFilterAndProximitySpec<T> : FluentSelectSpecification<T, object> where T : FluentEntity
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string ColumnName { get; set; }

        private int Tolerance { get; set; }

        public Filter[] Filters { get; set; }

        public bool IsList { get; set; }

        public TermByFilterAndProximitySpec<T> SetParameter(Filter[] filters, bool isList, string property, string term, int tolerance)
        {
            Filters = filters;
            IsList = isList;
            TableName = typeof(T).GetTableName();
            Term = term;

            if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(Term))
            {
                ColumnName = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Property not found '{typeof(T).Name}.{property}'");
            }

            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<object> Where(IQueryable<T> query)
        {
            IgnoreOrder = true;
            var expression = Filters.FiltersToExtression<T>();

            query = query
                     .Where(expression)
                     .GetInclusions(IsList);

            if (!string.IsNullOrWhiteSpace(ColumnName) && !string.IsNullOrWhiteSpace(Term))
            {
                query = query.WhereProximityText(Term, TableName, ColumnName, Tolerance);
            }

            return query.FluentDynamicProjectTo(Service);
        }

        public override IOrderedQueryable<object> Order(IQueryable<object> query) => throw new NotImplementedException();
    }
}
