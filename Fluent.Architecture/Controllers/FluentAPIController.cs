using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        // GET api/user/list
        [HttpGet]
        public object List()
        {
            return new { list = Service.List(CreateSpec<AllSpec<T>>()), pagination = LastRequestPagination };
        }

        // GET api/user/Filtered list
        [HttpPost]
        public object FilteredList(Filter[] filters)
        {
            return new { list = Service.FilteredList(filters), pagination = LastRequestPagination };
        }

        // GET api/user/Find?id=5
        [HttpGet]
        public T Find([FromQuery]T value)
        {
            return Service.Find(value, false);
        }

        // GET api/user/FindByTerm?term=myterm
        [HttpGet]
        public object FindByTerm(string term)
        {
            return new { term, list = Service.FindByTerm(term), pagination = LastRequestPagination };
        }

        // GET api/user/Count
        [HttpGet]
        public int Count()
        {
            return Service.Count();
        }

        // GET api/user/Exists/?id=5
        [HttpGet]
        public bool Exists(T value)
        {
            return Service.Exists(value);
        }

        // POST api/user/Add/
        [HttpPost]
        public T Add([FromBody] T value)
        {
            return Service.Add(value);
        }

        // POST api/user/AddRange
        [HttpPost]
        public void AddRange([FromBody] T[] values)
        {
            Service.AddRange(values);
        }

        // PUT api/user/Update
        [HttpPut]
        public T Update([FromBody] T value)
        {
            return Service.Update(value);
        }

        // PUT api/user/UpdateRange
        [HttpPut]
        public void UpdateRange([FromBody] T[] values)
        {
            Service.UpdateRange(values);
        }

        // DELETE api/user/Remove
        [HttpDelete]
        public void Remove([FromBody] T value)
        {
            Service.Remove(value);
        }

        // DELETE api/user/RemoveRange
        [HttpDelete]
        public void RemoveRange([FromBody] T[] values)
        {
            Service.RemoveRange(values);
        }
    }
}