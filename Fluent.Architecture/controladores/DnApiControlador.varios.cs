using dn32.infra.dados;
using dn32.infra.extensoes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dn32.infra.nucleo.controladores
{
    public partial class DnApiControlador<T>
    {
        [HttpGet]
        public virtual T Exemplo() => typeof(T).GetExampleValue() as T;

        [HttpGet]
        public virtual async Task<ResultadoPadrao<int>> Quantidade() =>
            await this.CrieResultadoAsync(await this.Servico.CountAsync());

        [HttpPost]
        public virtual async Task<ResultadoPadrao<int>> QuantidadePorFiltro([FromBody] Filtro[] filtros)
        {
            var especificacao = this.CriarEspecificacaoDeFiltros(filtros, true);
            var quantidade = await this.Servico.CountAsync(especificacao);
            return await this.CrieResultadoAsync(quantidade);
        }

        [HttpGet]
        [Route("/api/[controller]/EntidadeExiste")]
        public virtual async Task<ResultadoPadrao<bool>> EntidadeExisteGet([FromQuery] T entidade) =>
            await this.CrieResultadoAsync(await this.Servico.ExistsAsync(entidade));

        [HttpPost]
        [Route("/api/[controller]/EntidadeExiste")]
        public virtual async Task<ResultadoPadrao<bool>> EntidadeExistePost([FromBody] T entidade) =>
            await this.CrieResultadoAsync(await this.Servico.ExistsAsync(entidade));

        [HttpGet]
        public virtual string Formulario(bool usarLayoutCompacto = false) =>
            typeof(T).GetDnJsonSchema(usarLayoutCompacto).SerializarParaDnJson();
    }
}