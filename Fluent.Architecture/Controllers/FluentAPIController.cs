using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fluent.Architecture.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        #region MANY

        [HttpGet]
        public virtual DefaultPaginationResult List()
        {
            var spec = CreateSpec<FluentAllSpec<T>>().SetParameter(isList: true);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilter")]
        public virtual DefaultPaginationResult ListByFilterGet([FromQuery] Filter[] filters)
        {
            return InternalListByFilter(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilter")]
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
        public virtual DefaultResult FindByEntityGet([FromQuery] T value)
        {
            return Result(Service.Find(value, false));
        }

        [HttpPost]
        [Route("/api/[controller]/FindByEntity")]
        public virtual DefaultResult FindByEntityPost([FromBody] T value)
        {
            return Result(Service.Find(value, false));
        }

        [HttpGet]
        [Route("/api/[controller]/FindByFilter")]
        public virtual DefaultResult FindByFilterGet([FromQuery] Filter[] filters)
        {
            return InternalFindByFilter(filters);
        }

        [HttpPost]
        [Route("/api/[controller]/FindByFilter")]
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
        public virtual DefaultResult FindByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term, isList: false);
            var item = Service.SingleOrDefault(spec);
            return Result(item);
        }

        #endregion

        #region ANOTHER

        [HttpGet]
        public virtual T ExampleData()
        {
            return typeof(T).GetExampleValue() as T;
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
        [Route("/api/[controller]/ExistsByEntity")]
        public virtual DefaultResult ExistsByEntityGet([FromQuery] T value)
        {
            return Result(Service.Exists(value));
        }

        [HttpPost]
        [Route("/api/[controller]/ExistsByEntity")]
        public virtual DefaultResult ExistsByEntityPost([FromBody] T value)
        {
            return Result(Service.Exists(value));
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

        #endregion               

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