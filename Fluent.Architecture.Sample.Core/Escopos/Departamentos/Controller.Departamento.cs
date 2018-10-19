using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Core.Escopos.Departamentos;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample.Core.Controllers
{
   // [Route("api/[controller]")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class DepartamentoController: FluentController<Departamento>
    {
        [HttpGet]
        public string Get()
        {
            return "1";// return Json(Service.Add(entity));
        }

        [HttpGet]
        public string Get2()
        {
            return "2";// return Json(Service.Add(entity));
        }
    }
}
