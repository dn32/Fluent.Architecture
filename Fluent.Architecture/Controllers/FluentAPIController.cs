using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentEntity, new()
    {
        // GET api/controller
        [HttpGet]
        [ActionName("GetAll")]
        public List<T> Get()
        {
            return Service.List(CreateSpec<AllSpec<T>>());
        }

        // GET api/controller/id=5
        [HttpGet]
        [ActionName("Get")]
        public T Get([FromRoute] T value)
        {
            return Service.Find(value);
        }

        // GET api/controller/term=myterm
        [HttpGet]
        [ActionName("GetByTerm")]
        public List<T> Get(string term)
        {
            return Service.FindByTerm(term);
        }

        // POST api/controller
        [HttpPost]
        [ActionName("Post")]
        public T Post([FromBody] T value)
        {
            return Service.Add(value);
        }

        // POST api/controller
        [HttpPost]
        [ActionName("PostRange")]
        public T[] Post([FromBody] T[] values)
        {
            Service.AddRange(values);
            return values;
        }

        // PUT api/controller
        [HttpPut]
        [ActionName("Put")]
        public T Put([FromBody] T value)
        {
            return Service.Update(value);
        }

        // PUT api/controller
        [HttpPut]
        [ActionName("PutRange")]
        public void Put([FromBody] T[] values)
        {
            Service.UpdateRange(values);
        }

        // DELETE api/controller
        [HttpDelete]
        [ActionName("Delete")]
        public void Delete([FromBody] T value)
        {
            Service.Remove(value);
        }

        // DELETE api/controller
        [HttpDelete]
        [ActionName("DeleteRange")]
        public void Delete([FromBody] T[] values)
        {
            Service.RemoveRange(values);
        }
    }
}