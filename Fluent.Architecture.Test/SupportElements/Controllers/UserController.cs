#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Services;
using Fluent.Architecture.Test.SupportElements.Specifications;

#if NET461

#else
using Microsoft.AspNetCore.Mvc;
#endif

namespace Fluent.Architecture.Test.SupportElements.Controllers
{
    public class UserController : FluentController<User>
    {
        public new UserService Service => base.Service as UserService;

        public JsonResult UserByEmail(string email)
        {
            var user = Service.SpecOne(new UserByEmail(this, email));
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

        public void RemoveRange(UserByPassword spec)
        {
            Service.RemoveRange(spec);
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

        public JsonResult FindById(int id)
        {
            var user = Propagate(id);
            return Json(user);
        }

        public void NotFound()
        {
            Propagate(nameof(NotFound));
        }

        [NotPropagate]
        public void NotFound2()
        {
            Propagate();
        }

        public JsonResult SpecOne(FluentSpecification<User> spec)
        {
            var user = Service.SpecOne(spec);
            return Json(user);
        }

        public JsonResult SpecOne(FluentSelectSpecification<User, int> spec)
        {
            var userId = Service.SpecOne(spec);
            return Json(userId);
        }

        public JsonResult SpecOne(FluentSelectSpecification<User, UserStudent> spec)
        {
            var userStudent = Service.SpecOne(spec);
            return Json(userStudent);
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

        public JsonResult Exists(FluentSelectSpecification<User, int> spec)
        {
            var user = Service.Exists(spec);
            return Json(user);
        }

        public JsonResult Count(FluentSelectSpecification<User, int> spec)
        {
            var user = Service.Count(spec);
            return Json(user);
        }

        public JsonResult Spec(FluentSpecification<User> spec)
        {
            var user = Service.Spec(spec);
            return Json(user);
        }

        public JsonResult Spec(FluentSelectSpecification<User, int> spec)
        {
            var userId = Service.Spec(spec);
            return Json(userId);
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

        public JsonResult GetUserByEmail(string email)
        {
            var userStudent = Service.GetUserByEmail(email);
            return Json(userStudent);
        }
    }
}

#endif
