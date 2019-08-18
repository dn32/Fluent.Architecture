using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
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