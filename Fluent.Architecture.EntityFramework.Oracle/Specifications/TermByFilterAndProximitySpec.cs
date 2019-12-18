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
    public class TermByFilterAndProximitySpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string[] Columns { get; set; }

        private int Tolerance { get; set; }

        public Filter[] Filters { get; set; }

        public bool IsList { get; set; }

        public TermByFilterAndProximitySpec<T> SetParameter(Filter[] filters, bool isList, string[] properties, string term, int tolerance)
        {
            Filters = filters;
            IsList = isList;
            TableName = typeof(T).GetTableName();
            Term = term;

            if (properties?.Length > 0 && !string.IsNullOrWhiteSpace(Term))
            {
                Columns = properties.Select(property => typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Property not found '{typeof(T).Name}.{property}'")).ToArray();
            }

            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            IgnoreOrder = true;
            var expression = Filters.FiltersToExtression<T>();

            query = query
                     .Where(expression)
                     .GetInclusions(IsList);

            if (Columns?.Length > 0 && !string.IsNullOrWhiteSpace(Term))
            {
                query = query.WhereProximityText(Term, TableName, Columns, Tolerance);
            }

            return query.FluentDynamicProjectTo(Service);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query) => throw new NotImplementedException();
    }
}
