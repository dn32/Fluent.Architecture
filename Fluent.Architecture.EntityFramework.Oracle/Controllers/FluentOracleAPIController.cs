
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework.Oracle.Specifications;
using Microsoft.AspNetCore.Mvc;
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
        public virtual async Task<DefaultPaginationResult> ListByFilterAndProximityGet([FromQuery] Filter[] filters, string property, string term, int tolerance)
        {
            return await InternalListByFilterAndProximityAsync(filters, property, term, tolerance);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilterAndProximity")]
        [Description("Get a paginated list of items based on filters and text proximity")]
        public virtual async Task<DefaultPaginationResult> ListByFilterAndProximityPost([FromBody] Filter[] filters, string property, string term, int tolerance)
        {
            return await InternalListByFilterAndProximityAsync(filters, property, term, tolerance);
        }

        private async Task<DefaultPaginationResult> InternalListByFilterAndProximityAsync([FromBody] Filter[] filters, string property, string term, int tolerance)
        {
            var spec = CreateSpec<TermByFilterAndProximitySpec<T>>().SetParameter(filters, isList: true, property, term, tolerance);
            return await ResultAsync(Service.ListAsync(spec), LastRequestPagination);
        }
    }
}
