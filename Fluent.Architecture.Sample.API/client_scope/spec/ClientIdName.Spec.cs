using Fluent.Architecture.Specifications;
using System.Linq;

namespace Fluent.Architecture.Sample
{
    public class ClientIdNameSpec : FluentSelectSpecification<Client, ClientViewModel>
    {
        public override IQueryable<ClientViewModel> Where(IQueryable<Client> query)
        {
            return query.Select(x => new ClientViewModel { Name = x.Name, Id = x.Id });
        }

        public override IOrderedQueryable<ClientViewModel> Order(IQueryable<ClientViewModel> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}

