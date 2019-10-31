using Fluent.Architecture.Controllers;
using Fluent.Architecture.EntityFramework.Oracle.Specifications;
using Microsoft.AspNetCore.Mvc;
using Fluent.Architecture.Core.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fluent.Architecture.Core.Specifications;

namespace Fluent.Architecture.EntityFramework.Oracle.Controllers
{
    public class FluentOracleAPIController<T> : FluentAPIController<T> where T : FluentEntity, new()
    {
        [HttpGet]
        [FluentAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> FindByProximity(string property, string term, int tolerance)
        {
            var spec = CreateSpec<TermByProximitySpec<T>>().AddParameter(property, term, tolerance);
            var list = await Service.ListAsync(spec);
            return await ResultAsync<List<T>>(list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [FluentAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> ListByFilterAndProximityGet([FromBody] FiltersAndTerm filtersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(filtersAndTerm);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        [FluentAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> ListByFilterAndProximityPost([FromBody] FiltersAndTerm filtersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(filtersAndTerm);
        }

        private async Task<DefaultPaginationResult<List<T>>> InternalListByFilterAndProximityAsync(FiltersAndTerm filtersAndTerm)
        {
            var spec = CreateSpec<TermByFilterAndProximitySpec<T>>().SetParameter(filtersAndTerm.Filters, isList: true, filtersAndTerm.Property, filtersAndTerm.Term, filtersAndTerm.Tolerance);
            return await ResultAsync<List<T>>(Service.ListAsync(spec), LastRequestPagination);
        }
    }
}
