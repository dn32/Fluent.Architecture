using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace dn32.infra.nucleo.controladores
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class DnApiSomenteLeituraControlador<T> : DnApiControlador<T> where T : DnEntidade, new()
    {
        [NonAction]
        public override Task<ResultadoPadrao<T>> Adicionar([FromBody] T entidade) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<T>> AdicionarOuAtualizar([FromBody] T entidade) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<T[]>> AdicionarLista([FromBody] T[] entidades) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> Atualizar([FromBody] T entidade) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> AtualizarLista([FromBody] T[] entidades) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> Remover([FromBody] T entidade) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> RemoverLista([FromBody] T[] entidades) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> EliminarTudo([FromHeader] string APAGAR_TUDO = "false") => throw new InvalidOperationException();
    }
}