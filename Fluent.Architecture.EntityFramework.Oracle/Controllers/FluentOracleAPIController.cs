
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework.Oracle.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.EntityFramework.Oracle.Controllers
{
    public class FluentOracleAPIController<T> : FluentAPIController<T> where T : FluentEntity, new()
    {
        // GET api/user/FindByProximity?property=name&term=jon&tolerance=40
        [HttpGet]
        public virtual DefaultPaginationResult FindByProximity(string property, string term, int tolerance)
        {
            var spec = CreateSpec<TermByProximitySpec<T>>().AddParameter(property, term, tolerance);

            var list = Service.List(spec);
            return Result(list, LastRequestPagination);
        }
    }
}
