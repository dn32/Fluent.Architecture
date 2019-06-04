using Fluent.Architecture.Specifications;
using System.Linq;

namespace Max.AplicacaoDeTeste.bairro.spec
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
}

