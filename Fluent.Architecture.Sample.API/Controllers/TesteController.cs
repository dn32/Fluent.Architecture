using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample
{
    [Route("api/teste/[Action]")]
    public class TesteController : FluentController<EntidadeTeste>
    {
        [HttpGet]
        public object Add(EntidadeTeste entity)
        {
            entity.Name = "Teste";
            return Service.Add(entity);
        }

        [HttpGet]
        public object Get(EntidadeTeste entity)
        {
            return Service.Find(entity);
        }
    }
}
