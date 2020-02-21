using dn32.infra.extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.nucleo.extensoes;

namespace dn32.infra.Nucleo.Specifications
{
    public class DnFilterSpec<T> : DnSpecification<T> where T : DnEntidade
    {
        protected Filtro[] Filters { get; set; }

        public bool IsList { get; set; }

        public DnFilterSpec<T> SetParameter(Filtro[] filters, bool ehLista)
        {
            Filters = filters;
            IsList = ehLista;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = Filters.ConverterFiltrosParaExpressao<T>();

            return query
                 .Where(expression)
                 .GetInclusions(IsList)
                 .ProjetarDeFormaDinamica(Service);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.ProjetarDeFormaDinamicaOrdenada(Service);
        }
    }
}
