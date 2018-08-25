using System.Collections.Generic;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Models;
using Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Specifications.UserSpec;

#if NET461
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc;
#endif

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests.Controllers
{
    public class UserController : FluentController<User>
    {
        public JsonResult UserByEmail(string email)
        {
            var user = Service.SpecOne(new UserByEmail(Service, email));
            return Json(user);
        }

        public JsonResult Add(User user)
        {
            user = Service.Add(user);
            return Json(user);
        }

        public JsonResult AddRange(User[] users)
        {
             Service.AddRange(users);
            return Json(users);
        }

        public JsonResult Remove(User user)
        {
            user = Service.Remove(user);
            return Json(user);
        }

        public JsonResult RemoveRange(User[] users)
        {
            Service.RemoveRange(users);
            return Json(users);
        }

        public JsonResult Update(User user)
        {
            user = Service.Update(user);
            return Json(user);
        }

        public JsonResult Find(User user)
        {
            user = Service.Find(user);
            return Json(user);
        }

        public JsonResult SpecOne(FluentSpecification<User> spec)
        {
            var user = Service.SpecOne(spec);
            return Json(user);
        }

        public JsonResult Exists(FluentSpecification<User> spec)
        {
            var user = Service.Exists(spec);
            return Json(user);
        }

        public JsonResult Count(FluentSpecification<User> spec)
        {
            var user = Service.Count(spec);
            return Json(user);
        }

        public JsonResult Spec(FluentSpecification<User> spec)
        {
            var user = Service.Spec(spec);
            return Json(user);
        }

        public JsonResult Spec(FluentSpecification<User> spec, FluentPagination pagination)
        {
            var user = Service.Spec(spec, pagination);
            return Json(user);
        }

        public void RemoveRange(FluentSpecification<User> spec)
        {
            Service.RemoveRange(spec);
        }

    }
}