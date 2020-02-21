using dn32.infra.extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.nucleo.extensoes;

namespace dn32.infra.Nucleo.Specifications
{
    public class DnDynamicSpec<T> : DnSpecification<T> where T : DnEntidade
    {
        public string[] Fields { get; set; }

        public bool IsList { get; set; }

        public DnDynamicSpec<T> SetParameters(string[] fields, bool isList)
        {
            Fields = fields;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            query = query.GetInclusions(IsList);
            return query.ProjetarDeFormaDinamica(Service, Fields);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.ProjetarDeFormaDinamicaOrdenada(Service);
        }
    }
}
