
#if !NET461

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentIdEntity, new()
    {
        // GET api/controller
        [HttpGet]
        [FluentAllowAnonymous]
        public JsonResult Get()
        {
            var spec = CreateSpec<AllSpec<T>>();
            return Json(Service.List(spec));
        }

        // GET api/controller/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var spec = CreateSpec<SpecById<T>>().SetParameter(id);
            return Json(Service.FirstOrDefault(spec));
        }

        // POST api/controller
        [HttpPost]
        public JsonResult Post([FromBody] T value)
        {
            return Json(Service.Add(value));
        }

        // PUT api/controller/5
        [HttpPut]
        public JsonResult Put([FromBody] T value)
        {
            return Json(Service.Update(value));
        }

        // DELETE api/controller/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            return Json(Service.Remove(new T { Id = id }));
        }
    }
}

#endif
