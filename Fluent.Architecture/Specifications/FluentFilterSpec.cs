using Fluente.Arquitetura.Nucleo.Enumerator;
using Fluente.Arquitetura.Nucleo.Extensoes;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Specifications;
using System.Linq;

namespace Fluente.Arquitetura.Nucleo.Specifications
{
    public class FluenteFilterSpec<T> : FluenteSpecification<T> where T : FluenteEntity
    {
        protected Filter[] Filters { get; set; }

        public bool IsList { get; set; }

        public FluenteFilterSpec<T> SetParameter(Filter[] filters, bool isList)
        {
            Filters = filters;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = Filters.FiltersToExtression<T>();

            return query
                 .Where(expression)
                 .GetInclusions(IsList)
                 .FluenteDynamicProjectTo(Service);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.FluenteDynamicProjectToOrder(Service);
        }
    }
}
