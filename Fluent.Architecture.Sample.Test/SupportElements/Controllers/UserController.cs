
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Services;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class UserController : FluentController<User>
    {
        public new UserService Service => base.Service as UserService;

        public JsonResult Add(User entity)
        {
            entity = this.Service.Add(entity);
            return this.Json(entity);
        }

        public JsonResult AddRange(User[] entities)
        {
            this.Propagate(new object[] { entities });
            return this.Json(entities);
        }

        public JsonResult Remove(User entity)
        {
            return this.Json(this.Service.Remove(entity));
        }

        public JsonResult RemoveRange(User[] entities)
        {
            this.Service.RemoveRange(entities);
            return this.Json(entities);
        }

        public JsonResult Update(User entity)
        {
            return this.Json(this.Service.Update(entity));
        }

        public JsonResult Find(User entity)
        {
            return this.Json(this.Service.Find(entity));
        }

        public JsonResult Find(User entity, string language)
        {
            var entityFinded = Propagate(new object[] { entity, language });
            return this.Json(entityFinded);
        }

        public void RemoveRange(UserByPassword spec)
        {
            this.Service.RemoveRange(spec);
        }

        public void Test()
        {
            this.Propagate();
        }

        public void Test2()
        {
            this.Propagate();
        }

        public JsonResult UserByEmail(string email)
        {
            var user = this.Service.FirstOrDefault(new UserByEmail(this, email));
            return this.Json(user);
        }

        public JsonResult FindById(int id)
        {
            var user = this.Propagate(id);
            return this.Json(user);
        }

        public void NotFound()
        {
            this.PropagateMethod(nameof(this.NotFound));
        }

        // public void NotFound2()
        // {
        // Propagate();
        // }
        public JsonResult SpecOne(FluentSpecification<User> spec)
        {
            var user = this.Service.FirstOrDefault(spec);
            return this.Json(user);
        }

        public JsonResult SpecOne(FluentSelectSpecification<User, int> spec)
        {
            var userId = this.Service.FirstOrDefault(spec);
            return this.Json(userId);
        }

        public JsonResult SpecOne(FluentSelectSpecification<User, UserStudent> spec)
        {
            var userStudent = this.Service.FirstOrDefault(spec);
            return this.Json(userStudent);
        }

        public JsonResult Exists(FluentSpecification<User> spec)
        {
            var exists = this.Propagate<bool>(spec);
            return this.Json(exists);
        }

        public JsonResult Count(FluentSpecification<User> spec)
        {
            var user = this.Service.Count(spec);
            return this.Json(user);
        }

        public JsonResult Exists(FluentSelectSpecification<User, int> spec)
        {
            var user = this.Service.Exists(spec);
            return this.Json(user);
        }

        public JsonResult Count(FluentSelectSpecification<User, int> spec)
        {
            var user = this.Service.Count(spec);
            return this.Json(user);
        }

        public JsonResult List(FluentSpecification<User> spec)
        {
            var user = this.Service.List(spec);
            return this.Json(user);
        }

        public JsonResult List(FluentSelectSpecification<User, int> spec)
        {
            var userId = this.Service.ListSelect(spec);
            return this.Json(userId);
        }

        public JsonResult List(FluentSpecification<User> spec, FluentPagination pagination)
        {
            var user = this.Service.List(spec, pagination);
            return this.Json(user);
        }

        public void RemoveRange(FluentSpecification<User> spec)
        {
            this.Service.RemoveRange(spec);
        }

        public JsonResult GetUserByEmail(string email)
        {
            var userStudent = this.Service.GetUserByEmail(email);
            return this.Json(userStudent);
        }

        public JsonResult PropagateMethodTestA()
        {
            return this.Json(this.PropagateMethod(nameof(this.PropagateMethodTestA)));
        }

        public JsonResult PropagateMethodTestB(int id, string name)
        {
            return this.Json(this.PropagateMethod(nameof(this.PropagateMethodTestB), id, name));
        }

        public JsonResult PropagateMethodTestC(int id, string name)
        {
            return this.Json(this.PropagateMethod<Student>(nameof(this.PropagateMethodTestC), new object[] { id, name }));
        }

        public JsonResult PropagateMethodTestD(int parameter)
        {
            return this.Json(this.PropagateMethod(nameof(this.PropagateMethodTestD), parameter));
        }

        public JsonResult PropagateMethodTestE(int id)
        {
            return this.Json(this.PropagateMethod<Student>(nameof(this.PropagateMethodTestE), id));
        }

        // ===========================

        public JsonResult PropagateTestF()
        {
            return this.Json(this.Propagate());
        }

        public JsonResult PropagateTestG(int id, string name)
        {
            return this.Json(this.Propagate(new object[] { id, name }));
        }

        public JsonResult PropagateTestH(int id, string name)
        {
            return this.Json(this.Propagate<Student>(new object[] { id, name }));
        }

        public JsonResult PropagateTestI(int id)
        {
            return this.Json(this.Propagate<Student>(new object[] { id }));
        }

        public JsonResult PropagateTestJ(int id)
        {
            return this.Json(this.Propagate(new object[] { id }));
        }

        public void ParameterCountFail()
        {
            this.PropagateMethod("List", 1, 2, 3);
        }
    }
}


