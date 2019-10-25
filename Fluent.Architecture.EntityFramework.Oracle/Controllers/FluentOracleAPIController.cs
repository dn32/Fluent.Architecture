using Fluent.Architecture.Controllers;
using Fluent.Architecture.EntityFramework.Oracle.Specifications;
using Microsoft.AspNetCore.Mvc;
using Fluent.Architecture.Core.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Fluent.Architecture.EntityFramework.Oracle.Controllers
{
    public class FluentOracleAPIController<T> : FluentAPIController<T> where T : FluentEntity, new()
    {
        [HttpGet]
        public virtual async Task<DefaultPaginationResult> FindByProximity(string property, string term, int tolerance)
        {
            var spec = CreateSpec<TermByProximitySpec<T>>().AddParameter(property, term, tolerance);
            var list = await Service.ListAsync(spec);
            return await ResultAsync(list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        public virtual async Task<DefaultPaginationResult> ListByFilterAndProximityGet([FromBody] FiltersAndTerm filtersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(filtersAndTerm);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        public virtual async Task<DefaultPaginationResult> ListByFilterAndProximityPost([FromBody] FiltersAndTerm filtersAndTerm)
        {
            return await InternalListByFilterAndProximityAsync(filtersAndTerm);
        }

        private async Task<DefaultPaginationResult> InternalListByFilterAndProximityAsync(FiltersAndTerm filtersAndTerm)
        {
            var spec = CreateSpec<TermByFilterAndProximitySpec<T>>().SetParameter(filtersAndTerm.Filters, isList: true, filtersAndTerm.Property, filtersAndTerm.Term, filtersAndTerm.Tolerance);
            return await ResultAsync(Service.ListSelectAsync(spec), LastRequestPagination);
        }
    }
}
