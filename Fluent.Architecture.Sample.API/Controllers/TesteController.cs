using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample
{
    [Route("api/mysql/[Action]")]
    public class MySQLController : FluentController<EntidadeMySQL>
    {
        [HttpPost]
        public object Add([FromBody] EntidadeMySQL entity)
        {
            return Service.Add(entity);
        }

        [HttpGet]
        public object Get(EntidadeMySQL entity)
        {
            return Service.Find(entity);
        }
    }

    [Route("api/sql/[Action]")]
    public class SQLController : FluentController<EntidadeSqlServer>
    {
        [HttpPost]
        public object Add([FromBody] EntidadeSqlServer entity)
        {
            return Service.Add(entity);
        }

        [HttpGet]
        public object Get(EntidadeSqlServer entity)
        {
            return Service.Find(entity);
        }
    }
}
