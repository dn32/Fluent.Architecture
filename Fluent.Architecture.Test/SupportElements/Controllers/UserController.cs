#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Test.SupportElements.Services;
using Fluent.Architecture.Test.SupportElements.Specifications;

namespace Fluent.Architecture.Test.SupportElements.Controllers
{
    public class UserController : FluentFullController<User>
    {
        public new UserService Service => base.Service as UserService;

        public void RemoveRange(UserByPassword spec)
        {
            Service.RemoveRange(spec);
        }

        public void Test()
        {
            Propagate();
        }
        public void Test2()
        {
            Propagate();
        }

        public JsonResult UserByEmail(string email)
        {
            var user = Service.SpecOne(new UserByEmail(this, email));
            return Json(user);
        }

        public JsonResult FindById(int id)
        {
            var user = Propagate(id);
            return Json(user);
        }

        public void NotFound()
        {
            PropagateMethod(nameof(NotFound));
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
            var exists = Propagate<bool>(spec);
            return Json(exists);
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
        



        public JsonResult PropagateMethodTestA()
        {
            return Json(PropagateMethod(nameof(PropagateMethodTestA)));
        }
        
        public JsonResult PropagateMethodTestB(int id, string name)
        {
            return Json(base.PropagateMethod(nameof(PropagateMethodTestB), new object[] { id, name }));
        }
        
        public JsonResult PropagateMethodTestC(int id, string name)
        {
            return Json(base.PropagateMethod<Student>(nameof(PropagateMethodTestC), new object[]{ id, name }));
        }
        
        public JsonResult PropagateMethodTestD(int parameter)
        {
            return Json(PropagateMethod(nameof(PropagateMethodTestD), parameter));
        }
        
        public JsonResult PropagateMethodTestE(int id)
        {
            return Json(base.PropagateMethod<Student>(nameof(PropagateMethodTestE), id));
        }

        //===========================
        
        public JsonResult PropagateTestF()
        {
            return Json(base.Propagate());
        }
        
        public JsonResult PropagateTestG(int id, string name)
        {
            return Json(base.Propagate(new object[] { id, name }));
        }
        
        public JsonResult PropagateTestH(int id, string name)
        {
            return Json(base.Propagate<Student>(new object[] { id, name }));
        }
        
        public JsonResult PropagateTestI(int id)
        {
            return Json(base.Propagate<Student>(new object[] { id }));
        }

        public JsonResult PropagateTestJ(int id)
        {
            return Json(base.Propagate(new object[] { id }));
        }

    }
}

#endif
