using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Nucleo.Extensoes;
using Fluente.Arquitetura.Specifications;
using System.Linq;
using dn32.infra.dados;

namespace Fluente.Arquitetura.Nucleo.Specifications
{
    public class FluenteAllSpec<T> : FluenteSpecification<T> where T : FluenteEntidade
    {
        public bool IsList { get; set; } = true;

        public FluenteAllSpec<T> SetParameter(bool isList)
        {
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            return query
                    .GetInclusions(IsList)
                    .FluenteDynamicProjectTo(Service);

        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.FluenteDynamicProjectToOrder(Service);
        }
    }
}
