using dn32.infra.dados;
using dn32.infra.nucleo.atributos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using dn32.infra.Nucleo.Specifications;

namespace dn32.infra.nucleo.controladores
{
    public partial class DnApiControlador<T>
    {
        [HttpGet]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> Listar()
        {
            var especificacao = this.CriarEspecificacaoDeConsultarTudo();
            var lista = await this.Servico.ListarAsync(especificacao);
            return await this.CrieResultadoAsync(lista, this.PaginacaoDaUltimaRequisicao);
        }

        [HttpGet]
        [Route("/api/[controller]/ListarPorFiltro")]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListarPorFiltroGet([FromQuery] Filtro[] filtros)
        {
            return await this.ListarPorFiltroInterno(filtros);
        }

        [HttpPost]
        [Route("/api/[controller]/ListarPorFiltro")]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListarPorFiltroPost([FromBody] Filtro[] filtros)
        {
            return await this.ListarPorFiltroInterno(filtros);
        }

        [HttpGet]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginadoComTermo<List<T>>> ListarPorTermo(string termo)
        {
            var especificacao = this.CriarEspecificacaoPorTermo(termo, ehLista: false);
            var lista = await this.Servico.ListarAsync(especificacao);
            return await this.CrieResultadoAsync(lista, this.PaginacaoDaUltimaRequisicao, termo);
        }

        protected virtual async Task<ResultadoPadraoPaginado<List<T>>> ListarPorFiltroInterno([FromBody] Filtro[] filtros)
        {
            var especificacao = this.CriarEspecificacaoDeFiltros(filtros, false);
            var lista = await this.Servico.ListarAsync(especificacao);
            return await this.CrieResultadoAsync(lista, this.PaginacaoDaUltimaRequisicao);
        }

        private DnAllSpec<T> CriarEspecificacaoDeConsultarTudo() => this.CriarEspecificacao<DnAllSpec<T>>().SetParameter(isList: true);
    }
}