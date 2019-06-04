using Fluent.Architecture.Specifications;
using System;
using System.Linq;

namespace Fluent.Architecture.Sample
{
    public class ClientByNameSpec : FluentSpecification<Client>
    {
        public string Name { get; set; }

        public ClientByNameSpec AddParameter(string name)
        {
            Name = name;
            return this;
        }

        public override IQueryable<Client> Where(IQueryable<Client> query)
        {
            var gestores = Get<Client>().ToList();
            
            return query.Where(x => x.Name.Contains(Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override IOrderedQueryable<Client> Order(IQueryable<Client> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}

