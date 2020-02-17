using Fluente.Arquitetura.Controllers;
using Fluente.Arquitetura.EntityFramework.Oracle.Specifications;
using Microsoft.AspNetCore.Mvc;
using Fluente.Arquitetura.Nucleo.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fluente.Arquitetura.Nucleo.Specifications;
using System.ComponentModel.DataAnnotations;

namespace Fluente.Arquitetura.EntityFramework.Oracle.Controllers
{
    public class FluenteOracleAPIController<T> : FluenteAPIController<T> where T : FluenteEntidade, new()
    {
        [HttpGet]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> FindByProximity(
            [Description("The properties whose value will be compared")] string[] properties,
            [Description("The term to use as a comparator")] string Term,
            [Description("The required acceptance percentage. The higher the value, the more demanding")][Range(0,100)]
            int Tolerance
        )
        {
            var spec = CreateSpec<TermByProximitySpec<T>>().AddParameter(properties, Term, Tolerance);
            var list = await Service.ListAsync(spec);
            return await ResultAsync<List<T>>(list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListByFilterAndProximityGet([FromBody, Description("The query object")] FiltersAndTerm FiltersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(FiltersAndTerm);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<ResultadoPadraoPaginado<List<T>>> ListByFilterAndProximityPost([FromBody, Description("The query object")] FiltersAndTerm FiltersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(FiltersAndTerm);
        }

        private async Task<ResultadoPadraoPaginado<List<T>>> InternalListByFilterAndProximityAsync(FiltersAndTerm filtersAndTerm)
        {
            var spec = CreateSpec<TermByFilterAndProximitySpec<T>>().SetParameter(filtersAndTerm.Filters, isList: true, filtersAndTerm.Properties, filtersAndTerm.Term, filtersAndTerm.Tolerance);
            var list = await Service.ListAsync(spec);
            return await ResultAsync<List<T>>(list, LastRequestPagination);
        }
    }
}
