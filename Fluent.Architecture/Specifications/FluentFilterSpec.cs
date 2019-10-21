using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentFilterSpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        protected Filter[] Filters { get; set; }

        public bool IsList { get; set; }

        public FluentFilterSpec<T> SetParameter(Filter[] filters, bool isList)
        {
            Filters = filters;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = Filters.FiltersToExtression<T>();
            query = query.Where(expression);
            query = query.GetInclusions(IsList);
            return query;
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x);
        }
    }
}
