using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentFilterSpec<T> : FluentSelectSpecification<T, object> where T : FluentEntity
    {
        protected Filter[] Filters { get; set; }

        public bool IsList { get; set; }

        public FluentFilterSpec<T> SetParameter(Filter[] filters, bool isList)
        {
            Filters = filters;
            IsList = isList;
            return this;
        }

        public override IQueryable<object> Where(IQueryable<T> query)
        {
            var expression = Filters.FiltersToExtression<T>();

            return query
                 .Where(expression)
                 .GetInclusions(IsList)
                 .FluentDynamicProjectTo(Service);
        }

        public override IOrderedQueryable<object> Order(IQueryable<object> query)
        {
            return query.FluentDynamicProjectToOrder(Service);
        }
    }
}
