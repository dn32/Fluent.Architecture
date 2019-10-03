using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
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
            query = query.GetInclusions(IsList);
            query = query.Where(expression);
            return query;
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x);
        }
    }
}
