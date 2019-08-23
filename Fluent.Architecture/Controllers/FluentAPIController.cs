using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        private static readonly Random Random = new Random();

        private static int NextRandom(int max)
        {
            return Random.Next(0, max);
        }

        [HttpGet]
        public T ExampleData()
        {
            string GetValue(int size)
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[size];

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[NextRandom(chars.Length - 1)];
                }

                return new string(stringChars);
            }

            var obj = new T();
            foreach (var property in obj.GetType().GetProperties())
            {
                var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
                if (property.PropertyType == typeof(string))
                {
                    if (maxLengthAttribute?.Length != null)
                    {
                        property.SetValue(obj, GetValue(maxLengthAttribute.Length));
                    }
                    else
                    {
                        property.SetValue(obj, GetValue(3));
                    }
                }
                else if (property.PropertyType.IsNullableEnum())
                {
                    var firstEnum = Enum.GetValues(property.PropertyType).GetValue(1);
                    property.SetValue(obj, firstEnum);
                }
                else if (property.PropertyType.IsNumeric())
                {
                    var maxLengthAttribute2 = property.GetCustomAttribute<RangeAttribute>()?.Maximum as int?;
                    object MaxValue = null;

                    if (maxLengthAttribute2 != null)
                    {
                        MaxValue = NextRandom(maxLengthAttribute2.Value);
                    }
                    else
                    {
                        MaxValue = property.PropertyType.GetField("MaxValue").GetValue(null);
                    }

                    if (MaxValue as long? > int.MaxValue) { MaxValue = int.MaxValue; }
                    var maxValueInt = (Convert.ChangeType(MaxValue, typeof(int), CultureInfo.InvariantCulture) ?? int.MaxValue) as int?;
                    var value = NextRandom(maxValueInt.Value);
                    var objectValue = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
                    property.SetValue(obj, objectValue);
                }
            }

            return obj;
        }

        [HttpGet]
        public virtual string Schema()
        {
            var type = typeof(T);
            var settings = new JsonSchemaGeneratorSettings { GenerateExamples = true };
            var schema = JsonSchema.FromType(type, settings);
            schema.SchemaVersion = "http://json-schema.org/schema#";
            schema.Id = $"{Request.Scheme}://{Request.Host}{Request.Path}{type.Name}";
            var properties = type.GetRuntimeProperties().ToList();

            foreach (var jsonProperty in schema.Properties)
            {
                var property = properties.FirstOrDefault(x =>
 x.Name.Equals(jsonProperty.Value.Name, StringComparison.InvariantCultureIgnoreCase) ||
 x.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName?.Equals(jsonProperty.Value.Name, StringComparison.InvariantCultureIgnoreCase) == true);

                jsonProperty.Value.Id = $"#{type.Name}/{jsonProperty.Value.Name}";
                jsonProperty.Value.ExtensionData.Add("property", $"{property?.Name}");
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
            return Result(Service.Count());
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