using dn32.infra.dados;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dn32.infra.nucleo.controladores
{
    public partial class DnApiControlador<T>
    {
        [HttpGet]
        [Route("/api/[controller]/BuscarPorEntidade")]
        public virtual async Task<ResultadoPadrao<T>> BuscarPorEntidadeGet([FromQuery] T entidade)
        {
            return await this.CrieResultadoAsync(await this.Servico.BuscarAsync(entidade, false));
        }

        [HttpPost]
        [Route("/api/[controller]/BuscarPorEntidade")]
        public virtual async Task<ResultadoPadrao<T>> BuscarPorEntidadePost([FromBody] T entidade)
        {
            return await this.CrieResultadoAsync(await this.Servico.BuscarAsync(entidade, false));
        }

        [HttpGet]
        [Route("/api/[controller]/BuscarPorFiltro")]
        [DnActionAtributo(EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadrao<T>> BuscarPorFiltroGet([FromQuery] Filtro[] filtros)
        {
            var especificacao = this.CriarEspecificacaoDeFiltros(filtros, false);
            var item = await this.Servico.UnicoOuPadraoAsync(especificacao);
            return await this.CrieResultadoAsync(item);
        }

        [HttpPost]
        [Route("/api/[controller]/BuscarPorFiltro")]
        [DnActionAtributo(EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadrao<T>> BuscarPorFiltroPost([FromBody] Filtro[] filtros)
        {
            var especificacao = this.CriarEspecificacaoDeFiltros(filtros, false);
            var item = await this.Servico.UnicoOuPadraoAsync(especificacao);
            return await this.CrieResultadoAsync(item);
        }

        [HttpGet]
        [DnActionAtributo(EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadrao<T>> BuscarPorTermo(string termo)
        {
            var especificacao = this.CriarEspecificacaoPorTermo(termo, ehLista: false);
            var item = await this.Servico.UnicoOuPadraoAsync(especificacao);
            return await this.CrieResultadoAsync(item);
        }

        private TermSpec<T> CriarEspecificacaoPorTermo(string termo, bool ehLista) =>
             this.CriarEspecificacao<TermSpec<T>>().SetParameter(termo, ehLista);

        private DnFilterSpec<T> CriarEspecificacaoDeFiltros(Filtro[] filtros, bool ehLista) =>
              this.CriarEspecificacao<DnFilterSpec<T>>().SetParameter(filtros, ehLista);
    }
}
