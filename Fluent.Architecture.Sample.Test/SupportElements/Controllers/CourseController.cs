using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class CourseController : FluentGlobalizedController<Course>
    {
        public JsonResult Add(Course entity)
        {
            entity = this.Service.Add(entity);
            return this.Json(entity);
        }

        public JsonResult AddRange(Course[] entities)
        {
            this.Propagate(new object[] { entities });
            return this.Json(entities);
        }

        public JsonResult Remove(Course entity)
        {
            return this.Json(this.Service.Remove(entity));
        }

        public JsonResult RemoveRange(Course[] entities)
        {
            this.Service.RemoveRange(entities);
            return this.Json(entities);
        }

        public JsonResult Update(Course entity)
        {
            return this.Json(this.Service.Update(entity));
        }

        public JsonResult Find(Course entity)
        {
            return this.Json(this.Service.Find(entity));
        }

        public JsonResult Find(Course entity, string language)
        {
            var entityFinded = Propagate(new object[] { entity, language });
            return this.Json(entityFinded);
        }

        public JsonResult FirstOrDefault(string language)
        {
            var entityFinded = Propagate(new object[] { language });
            return this.Json(entityFinded);
        }

        public JsonResult List(string language)
        {
            var entityFinded = PropagateList(new object[] { language });
            return this.Json(entityFinded);
        }

        public JsonResult List(FluentPagination pagination, string language)
        {
            var spec = new CourseAllSpec(this);
            var entityFinded = PropagateList(new object[] {spec, pagination, language });
            return this.Json(entityFinded);
        }

        public JsonResult FirstOrDefaultSpec(string language)
        {
            var entityFinded = PropagateMethod("FirstOrDefault", new object[] {new CourseAllSpec(this), language });
            return this.Json(entityFinded);
        }
    }
}