using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fluent.Architecture.Sample
{
    [Route("api/[Controller]/[Action]")]
    public class ClientController : FluentController<Client>
    {
        [HttpPost]
        public object Add([FromBody] Client entity)
        {
            return Service.Add(entity);
        }

        [HttpPost]
        public object AddRange([FromBody] Client[] entities)
        {
            Service.AddRange(entities);
            return entities;
        }

        [HttpGet]
        public object GetAll()
        {
            var pag = new FluentPagination(0);
            return Service.List(CreateSpec<AllSpec<Client>>(), pag);
        }

        [HttpGet]
        public object GetAllNameId()
        {
            return Service.ListSelect(CreateSpec<ClientIdNameSpec>());
        }

        [HttpPost]
        public object Find(Client entity)
        {
            return Service.Find(entity);
        }

        [HttpGet]
        public object GetByName(string name)
        {
            var spec = CreateSpec<ClientByNameSpec>().AddParameter(name);
            return Service.List(spec);
        }
    }









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
            return query.Where(x => x.Name.Contains(Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override IOrderedQueryable<Client> Order(IQueryable<Client> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }

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

