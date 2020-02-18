using Fluente.Arquitetura.Nucleo.Extensoes;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Specifications;
using System.Linq;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Specifications
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
