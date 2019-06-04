using Fluent.Architecture.Specifications;
using System.Linq;

namespace Max.AplicacaoDeTeste.bairro.spec
{
    public class BairroCodigoDescricaoSpec : FluentSelectSpecification<Bairro, BairroCodigoDescricaoViewModel>
    {
        public override IQueryable<BairroCodigoDescricaoViewModel> Where(IQueryable<Bairro> query)
        {
            return query.Select(x => new BairroCodigoDescricaoViewModel { Codigo = x.CODBAIRRO, Descricao = x.DESCRICAO });
        }

        public override IOrderedQueryable<BairroCodigoDescricaoViewModel> Order(IQueryable<BairroCodigoDescricaoViewModel> query)
        {
            return query.OrderBy(x => x.Descricao);
        }
    }
}

