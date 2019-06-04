using Fluent.Architecture.Sample.API;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample
{
    [Route("api/[Controller]/[Action]")]
    public class BairroController : BaseController<Bairro>
    {
        [HttpGet]
        public object FirstOrDefault()
        {
            return Service.FirstOrDefault(CreateSpec<BairroAll>());
        }

        [HttpGet]
        public object List()
        {
            return Service.List(CreateSpec<BairroAll>());
        }

        [HttpGet]
        public object List2()
        {
            return Service.ListSelect(CreateSpec<BairroCodigoDescricaoSpec>());
        }

        [HttpGet]
        public object Add([FromBody] Bairro entidade)
        {
            return Service.Add(entidade);
        }

        [HttpGet]
        public object Update([FromBody] Bairro entidade)
        {
            return Service.Update(entidade);
        }

        [HttpGet]
        public object Remove([FromBody] Bairro entidade)
        {
            return Service.Remove(entidade);
        }
    }
}

