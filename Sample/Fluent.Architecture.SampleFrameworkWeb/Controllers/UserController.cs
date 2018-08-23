using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.SampleFrameworkWeb.Models;
using Fluent.Architecture.SampleFrameworkWeb.Specifications.UserSpec;

namespace Fluent.Architecture.SampleFrameworkWeb.Controllers
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