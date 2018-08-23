using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Models;
using Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Specifications.UserSpec;

namespace Fluent.Architecture.FrameworkTest.IntegrationTest.UserTests.Controllers
{
    public class UserController : FluentController<User>
    {
        public JsonResult UserByEmail(string email)
        {
            var user = Service.SpecOne(new UserByEmail(Service, email));
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(User user)
        {
            user = Service.Add(user);
            return Json(user);
        }

        public JsonResult Remove(User user)
        {
            user = Service.Remove(user);
            return Json(user);
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
    }
}