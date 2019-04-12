using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample
{
    [Route("api/teste/[Action]")]
    public class TesteController : FluentController<EntidadeTeste>
    {
        [HttpPost]
        public object Add([FromBody] EntidadeTeste entity)
        {
            return Service.Add(entity);
        }

        [HttpGet]
        public object Get(EntidadeTeste entity)
        {
            return Service.Find(entity);
        }
    }
}
