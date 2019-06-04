using Max.AplicacaoDeTeste.bairro.spec;
using Max.Infraestrutura.ClassesBase;
using Microsoft.AspNetCore.Mvc;

namespace Max.AplicacaoDeTeste.bairro
{
    [Route("api/[Controller]")]
    public class BairroController : MaxControllerAPI<Bairro>
    {
        [HttpGet("Quantidade")]
        public object Quantidade()
        {
            return Service.Count(CreateSpec<BairroAll>());
        }

        [HttpGet("ListarTodos")]
        public object ListarTodos()
        {
            return Service.List(CreateSpec<BairroAll>());
        }

        [HttpGet("ListarCodigoDescricao")]
        public object ListarCodigoDescricao()
        {
            return Service.ListSelect(CreateSpec<BairroCodigoDescricaoSpec>());
        }
    }
}

