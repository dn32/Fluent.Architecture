using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using Fluent.Architecture.Core.Models;

namespace Fluent.Architecture.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        #region MANY

        [HttpGet]
        [Description("Get a paged list of all items")]
        public virtual async Task<DefaultPaginationResult> List()
        {
            var spec = CreateSpec<FluentAllSpec<T>>().SetParameter(isList: true);
            var list = Service.ListSelectAsync(spec);
            return await ResultAsync(await list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        public virtual async Task<DefaultPaginationResult> ListByFilterGet([FromQuery] Filter[] filters)
        {
            return await InternalListByFilterAsync(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        public virtual async Task<DefaultPaginationResult> ListByFilterPostAsync([FromBody] Filter[] filters)
        {
            return await InternalListByFilterAsync(filters);
        }

        protected async Task<DefaultPaginationResult> InternalListByFilterAsync([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return await ResultAsync(await Service.ListSelectAsync(spec), LastRequestPagination);
        }

        [HttpGet]
        [Description("Get a paginated list of items based on a term")]
        public virtual async Task<DefaultPaginationTermResult> ListByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term, isList: true);
            var list = Service.ListSelectAsync(spec);
            return await ResultAsync(await list, LastRequestPagination, term);
        }

        #endregion

        #region ONE

        [HttpGet]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual async Task<DefaultResult> FindByEntityGet([FromQuery] T value)
        {
            return await ResultAsync(await Service.FindAsync(value, false));
        }

        [HttpPost]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual async Task<DefaultResult> FindByEntityPost([FromBody] T value)
        {
            return await ResultAsync(await Service.FindAsync(value, false));
        }

        [HttpGet]
        [Route("/api/[controller]/FindByFilter")]
        [Description("Get an item based on its filters")]
        public virtual async Task<DefaultResult> FindByFilterGet([FromQuery] Filter[] filters)
        {
            return await InternalFindByFilterAsync(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/FindByFilter")]
        [Description(" Get an item based on filters")]
        public virtual async Task<DefaultResult> FindByFilterPost([FromBody] Filter[] filters)
        {
            return await InternalFindByFilterAsync(filters);
        }

        private async Task<DefaultResult> InternalFindByFilterAsync([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: false);
            var item = await Service.SingleOrDefaultSelectAsync(spec);
            return await ResultAsync(item);
        }

        [HttpGet]
        [Description(" Get an item based on a term")]
        public virtual async Task<DefaultResult> FindByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term, isList: false);
            var item = Service.SingleOrDefaultSelectAsync(spec);
            return await ResultAsync(await item);
        }

        #endregion

        #region ANOTHER

        [HttpGet]
        [Description("Get an example of the item")]
        public virtual T ExampleData()
        {
            return typeof(T).GetExampleValue() as T;
        }

        [HttpGet]
        [Description("Get total amount of items")]
        public virtual async Task<DefaultResult> Count()
        {
            return await ResultAsync(await Service.CountAsync());
        }

        [HttpPost]
        [Description("Get the number of items based on filters.")]
        public virtual async Task<DefaultResult> CountByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return await ResultAsync(await Service.CountSelectAsync(spec));
        }

        [HttpGet]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Get the number of items based on filters.")]
        public virtual async Task<DefaultResult> ExistsByEntityGet([FromQuery] T value)
        {
            return await ResultAsync(await Service.ExistsAsync(value));
        }

        [HttpPost]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an item exists based on its IDs")]
        public virtual async Task<DefaultResult> ExistsByEntityPost([FromBody] T value)
        {
            return await ResultAsync(await Service.ExistsAsync(value));
        }

        [HttpGet]
        [Description("Get item type schema")]
        public virtual string JsonForm(bool tablet = false)
        {
            return typeof(T).GetFluentJsonSchema(tablet).ToFluentJson();
        }

        #endregion               

        // POST api/user/Add/
        [HttpPost]
        [Description("Add an item")]
        public virtual async Task<DefaultResult> Add([FromBody] T value)
        {
            return await ResultAsync(await Service.AddAsync(value));
        }

        // POST api/user/AddOrUpdate/
        [HttpPost]
        [Description("Add an item")]
        public virtual async Task<DefaultResult> AddOrUpdate([FromBody] T value)
        {
            return await ResultAsync(await Service.AddOrUpdateAsync(value));
        }

        // POST api/user/AddRange
        [HttpPost]
        [Description("Add or update an item")]
        public virtual async Task<DefaultResult> AddRange([FromBody] T[] values)
        {
            await Service.AddRangeAsync(values);
            return await ResultAsync(values);
        }

        // PUT api/user/Update
        [HttpPut]
        [Description("Update an item")]
        public virtual async Task<DefaultResult> Update([FromBody] T value)
        {
            await Service.UpdateAsync(value);
            return await ResultAsync(true);
        }

        // PUT api/user/UpdateRange
        [HttpPut]
        [Description("Update a list of items")]
        public virtual async Task<DefaultResult> UpdateRange([FromBody] T[] values)
        {
            await Service.UpdateRangeAsync(values);
            return await ResultAsync(true);
        }

        // DELETE api/user/Remove
        [HttpDelete]
        [Description("Remove item based on their identifiers")]
        public virtual async Task<DefaultResult> Remove([FromBody] T value)
        {
            await Service.RemoveAsync(value);
            return await ResultAsync(true);
        }

        // DELETE api/user/RemoveRange
        [HttpDelete]
        [Description("Remove a list of items based on their identifiers")]
        public virtual async Task<DefaultResult> RemoveRange([FromBody] T[] values)
        {
            await Service.RemoveRangeAsync(values);
            return await ResultAsync(true);
        }

        [HttpDelete]
        [Description("Physically deletes all elements of a set")]
        public virtual async Task<DefaultResult> Truncate([FromHeader] string ERASE_ALL_DATA = "false")
        {
            await Service.TruncateAsync(ERASE_ALL_DATA);
            return await ResultAsync(true);
        }
    }
}