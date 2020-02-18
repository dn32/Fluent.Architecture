using dn32.infra.nucleo.controladores;
using dn32.infra.EntityFramework.Oracle.Specifications;
using dn32.infra.Nucleo.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using dn32.infra.dados;
using dn32.infra.nucleo.atributos;

namespace dn32.infra.EntityFramework.Oracle.Controllers
{
    public class DnApiControladorOracle<T> : nucleo.controladores.DnApiControlador<T> where T : DnEntidade, new()
    {
        [HttpGet]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> FindByProximity(
            [Description("The properties whose valor will be compared")] string[] properties,
            [Description("The term to use as a comparator")] string Term,
            [Description("The EhRequerido acceptance percentage. The higher the valor, the more demanding")][Range(0,100)]
            int Tolerance
        )
        {
            var spec = this.CriarEspecificacao<TermByProximitySpec<T>>().AddParameter(properties, Term, Tolerance);
            var list = await this.Servico.ListarAsync(spec);
            return await this.CrieResultadoAsync<List<T>>(list, this.PaginacaoDaUltimaRequisicao);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListByFilterAndProximityGet([FromBody, Description("The query object")] FiltersAndTerm FiltersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(FiltersAndTerm);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [DnActionAtributo(Paginacao = true, EspecificacaoDinamica = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListByFilterAndProximityPost([FromBody, Description("The query object")] FiltersAndTerm FiltersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(FiltersAndTerm);
        }

        private async Task<ResultadoPadraoPaginado<List<T>>> InternalListByFilterAndProximityAsync(FiltersAndTerm filtersAndTerm)
        {
            var spec = this.CriarEspecificacao<TermByFilterAndProximitySpec<T>>().SetParameter(filtersAndTerm.Filters, isList: true, filtersAndTerm.Properties, filtersAndTerm.Term, filtersAndTerm.Tolerance);
            var list = await this.Servico.ListarAsync(spec);
            return await this.CrieResultadoAsync<List<T>>(list, this.PaginacaoDaUltimaRequisicao);
        }
    }
}
