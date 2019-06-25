// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
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
            Service.AddRange(entities);
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

        public void RemoveRange(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            this.Service.RemoveRange(spec);
        }

        public JsonResult Update(User entity)
        {
            return this.Json(this.Service.Update(entity));
        }

        public JsonResult Find(User entity)
        {
            return this.Json(this.Service.Find(entity));
        }

        //public JsonResult Find(User entity, string language)
        //{
        //    var entityFinded = Propagate(new object[] { entity, language });
        //    return this.Json(entityFinded);
        //}

        //public void Test()
        //{
        //    this.Propagate();
        //}

        //public void Test2()
        //{
        //    this.Propagate();
        //}

        public JsonResult UserByEmail(string email)
        {
            var user = this.Service.FirstOrDefault(CreateSpec<UserByEmailSpec>().DefineParams(email));
            return this.Json(user);
        }

        public JsonResult FindById(int id)
        {
            var user = Service.FirstOrDefault(CreateSpec<SpecFindById<User>>().DefineParameters(id));
            return this.Json(user);
        }

        //public void NotFound()
        //{
        //    this.PropagateMethod(nameof(this.NotFound));
        //}

        //public void NotFound2()
        //{
        //    Propagate();
        //}

        public JsonResult SpecOne(string email)
        {
            var spec = CreateSpec<UserByEmailSpec>().DefineParams(email);
            var userStudent = this.Service.FirstOrDefault(spec);
            return this.Json(userStudent);
        }

        public JsonResult SpecOneInt(string email)
        {
            var spec = CreateSpec<UserIdByEmailSpec>().DefineParams(email);
            var userStudent = this.Service.FirstOrDefaultSelect(spec);
            return this.Json(userStudent);
        }

        public JsonResult SpecOneUserAndStudent(string email)
        {
            var spec = CreateSpec<UserAndStudentByEmailSpec>().DefineParams(email);
            var userStudent = this.Service.FirstOrDefaultSelect(spec);
            return this.Json(userStudent);
        }

        public JsonResult FirstOrDefault()
        {
            var user = this.Service.FirstOrDefault();
            return this.Json(user);
        }

        public JsonResult Count()
        {
            var spec = CreateSpec<AllSpec<User>>();
            return Json(Service.Count(spec));
        }

        public JsonResult CountIdByPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            var user = this.Service.CountSelect(spec);
            return this.Json(user);
        }

        public JsonResult CountByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            var user = this.Service.Count(spec);
            return this.Json(user);
        }

        public JsonResult ExistsByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            var user = this.Service.Exists(spec);
            return this.Json(user);
        }

        public JsonResult ExistsIdByPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            var user = this.Service.Exists(spec);
            return this.Json(user);
        }

        public JsonResult CountByTelNumber(string number)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
            var user = this.Service.Count(spec);
            return this.Json(user);
        }

        public JsonResult ListByTelNumber(string number)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
            var user = this.Service.List(spec);
            return this.Json(user);
        }

        public JsonResult ListByTelNumber(string number, FluentPagination pagination)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
            var user = this.Service.List(spec, pagination);
            return this.Json(user);
        }

        public JsonResult ListByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            var user = this.Service.List(spec);
            return this.Json(user);
        }

        public JsonResult ListByIdPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            var user = this.Service.ListSelect(spec);
            return this.Json(user);
        }

        public JsonResult GetUserByEmail(string email)
        {
            var userStudent = this.Service.GetUserByEmail(email);
            return this.Json(userStudent);
        }
    }
}


