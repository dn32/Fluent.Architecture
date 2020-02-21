using dn32.infra.extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.nucleo.extensoes;

namespace dn32.infra.Nucleo.Specifications
{
    public class DnAllSpec<T> : DnSpecification<T> where T : DnEntidade
    {
        public bool IsList { get; set; } = true;

        public DnAllSpec<T> SetParameter(bool isList)
        {
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            return query
                    .GetInclusions(IsList)
                    .ProjetarDeFormaDinamica(Service);

        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.ProjetarDeFormaDinamicaOrdenada(Service);
        }
    }
}
