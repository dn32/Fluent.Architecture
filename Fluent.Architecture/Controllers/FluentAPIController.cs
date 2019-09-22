using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Fluent.Architecture.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        #region MANY

        [HttpGet]
        [Description("Get a paged list of all items")]
        public virtual DefaultPaginationResult List()
        {
            var spec = CreateSpec<FluentAllSpec<T>>().SetParameter(isList: true);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        public virtual DefaultPaginationResult ListByFilterGet([FromQuery] Filter[] filters)
        {
            return InternalListByFilter(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        public virtual DefaultPaginationResult ListByFilterPost([FromBody] Filter[] filters)
        {
            return InternalListByFilter(filters);
        }

        private DefaultPaginationResult InternalListByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return Result(Service.List(spec), LastRequestPagination);
        }

        [HttpGet]
        [Description("Get a paginated list of items based on a term")]
        public virtual DefaultPaginationTermResult ListByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term, isList: true);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination, term);
        }

        #endregion

        #region ONE

        [HttpGet]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual DefaultResult FindByEntityGet([FromQuery] T value)
        {
            return Result(Service.Find(value, false));
        }

        [HttpPost]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual DefaultResult FindByEntityPost([FromBody] T value)
        {
            return Result(Service.Find(value, false));
        }

        [HttpGet]
        [Route("/api/[controller]/FindByFilter")]
        [Description("Get an item based on its filters")]
        public virtual DefaultResult FindByFilterGet([FromQuery] Filter[] filters)
        {
            return InternalFindByFilter(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/FindByFilter")]
        [Description(" Get an item based on filters")]
        public virtual DefaultResult FindByFilterPost([FromBody] Filter[] filters)
        {
            return InternalFindByFilter(filters);
        }

        private DefaultResult InternalFindByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: false);
            var item = Service.SingleOrDefault(spec);
            return Result(item);
        }

        [HttpGet]
        [Description(" Get an item based on a term")]
        public virtual DefaultResult FindByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term, isList: false);
            var item = Service.SingleOrDefault(spec);
            return Result(item);
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
        public virtual DefaultResult Count()
        {
            return Result(Service.Count());
        }

        [HttpPost]
        [Description("Get the number of items based on filters.")]
        public virtual DefaultResult CountByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return Result(Service.Count(spec));
        }

        [HttpGet]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Get the number of items based on filters.")]
        public virtual DefaultResult ExistsByEntityGet([FromQuery] T value)
        {
            return Result(Service.Exists(value));
        }

        [HttpPost]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an item exists based on its IDs")]
        public virtual DefaultResult ExistsByEntityPost([FromBody] T value)
        {
            return Result(Service.Exists(value));
        }

        [HttpGet]
        [Description("Get item type schema")]
        public virtual JsonResult JsonForm(bool tablet = false)
        {
            var data = typeof(T).GetFluentJsonSchema(tablet);
            return Json(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None });
            //#if NETCOREAPP3_0
            //            return System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions { IgnoreNullValues = true });
            //#else
            //return JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None });
            //#endif
        }

        #endregion               

        // POST api/user/Add/
        [HttpPost]
        [Description("Add an item")]
        public virtual DefaultResult Add([FromBody] T value)
        {
            return Result(Service.Add(value));
        }

        // POST api/user/AddOrUpdate/
        [HttpPost]
        [Description("Add an item")]
        public virtual DefaultResult AddOrUpdate([FromBody] T value)
        {
            return Result(Service.AddOrUpdate(value));
        }

        // POST api/user/AddRange
        [HttpPost]
        [Description("Add or update an item")]
        public virtual DefaultResult AddRange([FromBody] T[] values)
        {
            Service.AddRange(values);
            return Result(values);
        }

        // PUT api/user/Update
        [HttpPut]
        [Description("Update an item")]
        public virtual DefaultResult Update([FromBody] T value)
        {
            Service.Update(value);
            return Result(true);
        }

        // PUT api/user/UpdateAlter
        [HttpPut]
        [Description("Updates an item based on another item's identifiers")]
        public virtual DefaultResult UpdateAlter([FromBody] UpdateAlter<T> value)
        {
            Service.UpdateAlter(value);
            return Result(true);
        }

        // PUT api/user/UpdateRange
        [HttpPut]
        [Description("Update a list of items")]
        public virtual DefaultResult UpdateRange([FromBody] T[] values)
        {
            Service.UpdateRange(values);
            return Result(true);
        }

        // DELETE api/user/Remove
        [HttpDelete]
        [Description("Remove item based on their identifiers")]
        public virtual DefaultResult Remove([FromBody] T value)
        {
            Service.Remove(value);
            return Result(true);
        }

        // DELETE api/user/RemoveRange
        [HttpDelete]
        [Description("Remove a list of items based on their identifiers")]
        public virtual DefaultResult RemoveRange([FromBody] T[] values)
        {
            Service.RemoveRange(values);
            return Result(true);
        }

        [HttpDelete]
        [Description("Physically deletes all elements of a set")]
        public virtual DefaultResult Truncate([FromHeader] string ERASE_ALL_DATA = "false")
        {
            Service.Truncate(ERASE_ALL_DATA);
            return Result(true);
        }
    }
}