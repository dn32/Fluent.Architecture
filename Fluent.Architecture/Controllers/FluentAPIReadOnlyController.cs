using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using Fluent.Architecture.Core.Models;
using System;

namespace Fluent.Architecture.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIReadOnlyController<T> : FluentAPIController<T> where T : FluentEntity, new()
    {
        [NonAction]
        public override Task<DefaultResult<T>> Add([Description("The entity you want to add"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<T>> AddOrUpdate([Description("The entity you want to add or update"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<T[]>> AddRange([Description("The entities you want to add"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<bool>> Update([Description("The entity you want to update"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<bool>> UpdateRange([Description("The entities you want to update"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<bool>> Remove([Description("The entity you want to remove"), FromBody] T Entity) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<bool>> RemoveRange([Description("The entities you want to remove"), FromBody] T[] Entities) => throw new InvalidOperationException();
        [NonAction]
        public override Task<DefaultResult<bool>> Truncate([Description("Confirmation that you really want to delete the data. If yes, enter \"Yes\""), FromHeader] string ERASE_ALL_DATA = "false") => throw new InvalidOperationException();
    }
}