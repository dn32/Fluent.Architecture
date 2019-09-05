using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;

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




        //       [HttpGet]
        //       public virtual string Schema()
        //       {
        //           var type = typeof(T);
        //           var settings = new JsonSchemaGeneratorSettings { GenerateExamples = true };
        //           var schema = JsonSchema.FromType(type, settings);
        //           schema.SchemaVersion = "http://json-schema.org/schema#";
        //           schema.Id = $"{Request.Scheme}://{Request.Host}{Request.Path}{type.Name}";
        //           var properties = type.GetRuntimeProperties().ToList();

        //           foreach (var jsonProperty in schema.Properties)
        //           {
        //               var property = properties.FirstOrDefault(x =>
        //x.Name.Equals(jsonProperty.Value.Name, StringComparison.InvariantCultureIgnoreCase) ||
        //x.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName?.Equals(jsonProperty.Value.Name, StringComparison.InvariantCultureIgnoreCase) == true);

        //               jsonProperty.Value.Id = $"#{type.Name}/{jsonProperty.Value.Name}";
        //               jsonProperty.Value.ExtensionData.Add("property", $"{property?.Name}");
        //           }

        //           return schema.ToJson();
        //       }

        // GET api/user/list
        [HttpGet]
        public virtual DefaultPaginationResult List()
        {
            return List(null);
        }


        // GET api/user/Find?id=5
        [HttpGet]
        public virtual DefaultResult Find([FromQuery]T value)
        {
            return Result(Service.Find(value, false));
        }

        // GET api/user/FindByTerm?term=myterm
        [HttpGet]
        public virtual DefaultPaginationTermResult FindByTerm(string term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(term);
            var list = Service.List(spec);
            return Result(list, LastRequestPagination, term);
        }

        // GET api/user/Count
        [HttpGet]
        public virtual DefaultResult Count([FromBody] Filter[] filters)
        {
            FluentSpecification<T> spec;
            if (filters != null && filters.Length > 0)
            {
                spec = CreateSpec<FilterSpec<T>>().SetParameter(filters);
            }
            else
            {
                spec = CreateSpec<AllSpec<T>>();
            }

            return Result(Service.Count(spec));
        }

        // GET api/user/Exists/?id=5
        [HttpGet]
        public virtual DefaultResult Exists(T value)
        {
            return Result(Service.Exists(value));
        }

        [HttpPost]
        public virtual DefaultPaginationResult List([FromBody] Filter[] filters)
        {
            FluentSpecification<T> spec;
            if (filters != null && filters.Length > 0)
            {
                spec = CreateSpec<FilterSpec<T>>().SetParameter(filters);
            }
            else
            {
                spec = CreateSpec<AllSpec<T>>();
            }

            var list = Service.List(spec);

            return Result(list, LastRequestPagination);
        }

        // GET api/user/Count
        [HttpPost]
        public virtual DefaultResult Count()
        {
            return Result(Service.Count());
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