using dn32.infra.dados;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dn32.infra.nucleo.controladores
{
    [Route("/api/[controller]/[action]")]
    [ApiController] // Nunca defina como abstrato, pois o controle de log espera essa classe como concreta
    public partial class DnApiControlador<T> : DnControlador<T> where T : DnEntidade, new()
    {
        [HttpPost]
        public virtual async Task<ResultadoPadrao<T>> Adicionar([FromBody] T entidade)
        {
            return await this.CrieResultadoAsync(await this.Servico.AddAsync(entidade));
        }

        [HttpPost]
        public virtual async Task<ResultadoPadrao<T>> AdicionarOuAtualizar([FromBody] T entidade)
        {
            return await this.CrieResultadoAsync(await this.Servico.AddOrUpdateAsync(entidade));
        }

        [HttpPost]
        public virtual async Task<ResultadoPadrao<T[]>> AdicionarLista([FromBody] T[] entidades)
        {
            await this.Servico.AddRangeAsync(entidades);
            return await this.CrieResultadoAsync(entidades);
        }

        [HttpPut]
        public virtual async Task<ResultadoPadrao<bool>> Atualizar([FromBody] T entidade)
        {
            await this.Servico.UpdateAsync(entidade);
            return await this.CrieResultadoAsync<bool>(true);
        }

        [HttpPut]
        public virtual async Task<ResultadoPadrao<bool>> AtualizarLista([FromBody] T[] entidades)
        {
            await this.Servico.UpdateRangeAsync(entidades);
            return await this.CrieResultadoAsync(true);
        }

        [HttpPost]
        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> Remover([FromBody] T entidade)
        {
            await this.Servico.RemoveAsync(entidade);
            return await this.CrieResultadoAsync(true);
        }

        [HttpPost]
        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> RemoverLista([FromBody] T[] entidades)
        {
            await this.Servico.RemoveRangeAsync(entidades);
            return await this.CrieResultadoAsync<bool>(true);
        }

        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> EliminarTudo([FromHeader] string APAGAR_TUDO = "false")
        {
            await this.Servico.TruncateAsync(APAGAR_TUDO);
            return await this.CrieResultadoAsync(true);
        }
    }
}