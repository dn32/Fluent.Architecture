using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class CourseController : FluentController<Course>
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
    }
}