using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        [HttpGet]
        public virtual T ExampleData()
        {
            return typeof(T).GetExampleValue() as T;
        }

        [HttpGet]
        public virtual DefaultPaginationResult List()
        {
            var spec = CreateSpec<FluentAllSpec<T>>().SetParameter(isList: true);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination);
        }

        [HttpGet]
        [HttpPost]
        public virtual DefaultResult Find(T value)
        {
            return Result(Service.Find(value, false));
        }

        [HttpPost]
        public virtual DefaultResult FindByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: false);
            var item = Service.SingleOrDefault(spec);
            return Result(item, LastRequestPagination);
        }

        [HttpGet]
        public virtual DefaultPaginationTermResult FindByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination, term);
        }

        [HttpGet]
        public virtual DefaultResult Count()
        {
            return Result(Service.Count());
        }

        [HttpPost]
        public virtual DefaultResult CountByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return Result(Service.Count(spec));
        }

        [HttpGet]
        public virtual DefaultResult Exists(T value)
        {
            return Result(Service.Exists(value));
        }

        [HttpPost]
        public virtual DefaultPaginationResult ListByFilter([FromBody] Filter[] filters)
        {
            var spec = CreateSpec<FluentFilterSpec<T>>().SetParameter(filters, isList: true);
            return Result(Service.List(spec), LastRequestPagination);
        }

        [HttpGet]
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

        // POST api/user/Add/
        [HttpPost]
        public virtual DefaultResult Add([FromBody] T value)
        {
            return Result(Service.Add(value));
        }

        // POST api/user/AddOrUpdate/
        [HttpPost]
        public virtual DefaultResult AddOrUpdate([FromBody] T value)
        {
            return Result(Service.AddOrUpdate(value));
        }

        // POST api/user/AddRange
        [HttpPost]
        public virtual DefaultResult AddRange([FromBody] T[] values)
        {
            Service.AddRange(values);
            return Result(values);
        }

        // PUT api/user/Update
        [HttpPut]
        public virtual DefaultResult Update([FromBody] T value)
        {
            Service.Update(value);
            return Result(true);
        }

        // PUT api/user/UpdateAlter
        [HttpPut]
        public virtual DefaultResult UpdateAlter([FromBody] UpdateAlter<T> value)
        {
            Service.UpdateAlter(value);
            return Result(true);
        }

        // PUT api/user/UpdateRange
        [HttpPut]
        public virtual DefaultResult UpdateRange([FromBody] T[] values)
        {
            Service.UpdateRange(values);
            return Result(true);
        }

        // DELETE api/user/Remove
        [HttpDelete]
        public virtual DefaultResult Remove([FromBody] T value)
        {
            Service.Remove(value);
            return Result(true);
        }

        // DELETE api/user/RemoveRange
        [HttpDelete]
        public virtual DefaultResult RemoveRange([FromBody] T[] values)
        {
            Service.RemoveRange(values);
            return Result(true);
        }

        [HttpDelete]
        public virtual DefaultResult Truncate([FromHeader] string ERASE_ALL_DATA = "false")
        {
            Service.Truncate(ERASE_ALL_DATA);
            return Result(true);
        }
    }
}