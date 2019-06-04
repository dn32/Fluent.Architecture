using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;

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
        public object Find(string email)
        {
            return Service.Find(new Client { Email = email });
        }

        [HttpGet]
        public object GetAll()
        {
            var pag = new FluentPagination(0);
            var list = Service.List(CreateSpec<AllSpec<Client>>(), pag);
            return new { list, pag };
        }

        [HttpGet]
        public object GetByName(string name)
        {
            var spec = CreateSpec<ClientByNameSpec>().AddParameter(name);
            return Service.List(spec);
        }

        [HttpGet]
        public object GetAllNameId()
        {
            return Service.ListSelect(CreateSpec<ClientIdNameSpec>());
        }
    }
}

