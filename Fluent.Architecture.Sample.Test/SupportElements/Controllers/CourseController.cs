//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using System.Web.Mvc;
//using Fluent.Architecture.Controllers;
//using Fluent.Architecture.Entities;
//using Fluent.Architecture.Model;
//using Fluent.Architecture.Sample.Test.SupportElements.Model;
//using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
//using Fluent.Architecture.Specifications;

//namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
//{
//    public class CourseController : FluentGlobalizedController<Course>
//    {
//        public JsonResult Add(Course entity)
//        {
//            entity = this.Service.Add(entity);
//            return this.Json(entity);
//        }

//        public JsonResult AddRange(Course[] entities)
//        {
//            Service.AddRange(entities);
//            return this.Json(entities);
//        }

//        public JsonResult Remove(Course entity)
//        {
//            return this.Json(this.Service.Remove(entity));
//        }

//        public JsonResult RemoveRange(Course[] entities)
//        {
//            this.Service.RemoveRange(entities);
//            return this.Json(entities);
//        }

//        public JsonResult Update(Course entity)
//        {
//            return this.Json(this.Service.Update(entity));
//        }

//        public JsonResult Find(Course entity)
//        {
//            return this.Json(this.Service.Find(entity));
//        }

//        public JsonResult Find(Course entity, string language)
//        {
//            var entityFinded = Service.Find(entity, language);
//            return this.Json(entityFinded);
//        }

//        public JsonResult FirstOrDefault(string language)
//        {
//            var entityFinded = Service.FirstOrDefault(language);
//            return this.Json(entityFinded);
//        }

//        public JsonResult List(string language)
//        {
//            var entityFinded = Service.List(language);
//            return this.Json(entityFinded);
//        }

//        public JsonResult List(FluentPagination pagination, string language)
//        {
//            var spec = CreateSpec<AllSpec<Course>>();
//            var entityFinded = Service.List(spec, pagination, language);
//            return this.Json(entityFinded);
//        }

//        public JsonResult FirstOrDefaultSpec(string language)
//        {
//            var entityFinded = Service.FirstOrDefault(language);
//            return this.Json(entityFinded);
//        }
//    }
//}