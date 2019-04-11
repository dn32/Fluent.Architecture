using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample
{
    [Route("api/teste")]
    public class TesteController : FluentController<EntidadeTeste>
    {
        [HttpGet]
        public void Add(EntidadeTeste entity)
        {
            Service.Add(entity);
        }
    }
}
