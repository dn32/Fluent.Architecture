using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using dn32.infra.dados;

namespace dn32.infra.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluenteAPIReadOnlyController<T> : FluenteAPIController<T> where T : FluenteEntidade, new()
    {
        [NonAction]
        public override Task<ResultadoPadrao<T>> Add([Description("The entity you want to add"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<T>> AddOrUpdate([Description("The entity you want to add or update"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<T[]>> AddRange([Description("The entities you want to add"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> Update([Description("The entity you want to update"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> UpdateRange([Description("The entities you want to update"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> Remove([Description("The entity you want to remove"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> RemoveRange([Description("The entities you want to remove"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<ResultadoPadrao<bool>> Truncate([Description("Confirmation that you really want to delete the data. If yes, enter \"Yes\""), FromHeader] string ERASE_ALL_DATA = "false") => throw new InvalidOperationException();
    }
}