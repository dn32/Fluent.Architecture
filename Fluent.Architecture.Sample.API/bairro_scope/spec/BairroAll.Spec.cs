using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Sample
{
    public class BairroAll : FluentSpecification<Bairro>
    {
        public override IQueryable<Bairro> Where(IQueryable<Bairro> query)
        {
            return query;
        }

        public override IOrderedQueryable<Bairro> Order(IQueryable<Bairro> query)
        {
            return query.OrderBy(x => x.CODBAIRRO);
        }
    }

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

