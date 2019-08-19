using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using NJsonSchema.Generation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        [HttpGet]
        public T ExampleData()
        {
            string GetValue(int size)
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[size];
                var valor = 0;

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[valor++];
                }

                return new string(stringChars);
            }

            var obj = new T();
            foreach (var properties in obj.GetType().GetProperties())
            {
                var minLengthAttribute = properties.GetCustomAttribute<MinLengthAttribute>();
                if (properties.PropertyType == typeof(string))
                {
                    if (minLengthAttribute?.Length != null)
                    {
                        properties.SetValue(obj, GetValue(minLengthAttribute.Length));
                    }
                    else
                    {
                        properties.SetValue(obj, GetValue(3));
                    }
                }
                else if (properties.PropertyType == typeof(int) || properties.PropertyType == typeof(long))
                {
                    properties.SetValue(obj, 3);
                }
                else if (properties.PropertyType == typeof(decimal))
                {
                    properties.SetValue(obj, 4.5m);
                }
            }

            return obj;
        }

        [HttpGet]
        public string Schema()
        {
            var type = typeof(T);
            var settings = new JsonSchemaGeneratorSettings { GenerateExamples = true };
            var schema = JsonSchema.FromType(type, settings);
            schema.SchemaVersion = "http://json-schema.org/schema#";
            schema.Id = $"{Request.Scheme}://{Request.Host}{Request.Path}{type.Name}";

            foreach (var p in schema.Properties)
            {
                p.Value.Id = $"#{type.Name}/{p.Value.Name}";
            }

            return schema.ToJson();
        }


        // GET api/user/list
        [HttpGet]
        public virtual DefaultPaginationResult List()
        {
            return List(null);
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
        [HttpPost]
        public virtual DefaultResult Count()
        {
            return Count(null);
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

        // POST api/user/Add/
        [HttpPost]
        public virtual DefaultResult Add([FromBody] T value)
        {
            return Result(Service.Add(value));
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
    }
}