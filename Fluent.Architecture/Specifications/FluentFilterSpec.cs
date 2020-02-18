using dn32.infra.Extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Specifications
{
    public class DnFilterSpec<T> : DnSpecification<T> where T : DnEntidade
    {
        protected Filtro[] Filters { get; set; }

        public bool IsList { get; set; }

        public DnFilterSpec<T> SetParameter(Filtro[] filters, bool isList)
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
                 .DnDynamicProjectTo(Service);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.DnDynamicProjectToOrder(Service);
        }
    }
}
