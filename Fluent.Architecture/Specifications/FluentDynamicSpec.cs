using dn32.infra.Extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Specifications
{
    public class FluenteDynamicSpec<T> : FluenteSpecification<T> where T : FluenteEntidade
    {
        public string[] Fields { get; set; }

        public bool IsList { get; set; }

        public FluenteDynamicSpec<T> SetParameters(string[] fields, bool isList)
        {
            Fields = fields;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            query = query.GetInclusions(IsList);
            return query.FluenteDynamicProjectTo(Service, Fields);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.FluenteDynamicProjectToOrder(Service);
        }
    }
}
