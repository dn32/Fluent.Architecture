
#if NETCOREAPP2_1

using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Fluent.Architecture.Controllers
{
  

    [Route("api/[controller]")]
    [ApiController]
    public class FluentAPIController<T> : FluentController<T> where T : FluentIdEntity, new()
    {
        // GET api/controller
        [HttpGet]
        public JsonResult Get()
        {
            return Json(Service.List(new AllSpec<T>(Service)));
        }

        // GET api/controller/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return Json(Service.FirstOrDefault(new SpecById<T>(Service, id)));
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
