using System.Web.Mvc;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Controllers
{
    // Todo item novo. Documentar
    public class FluentFullController<T> : FluentController<T>
        where T : BaseEntity, new()
    {
        public JsonResult Add(T entity)
        {
            return this.Json(this.Service.Add(entity));
        }

        public JsonResult AddRange(T[] entities)
        {
            this.Propagate(entities as object);
            return this.Json(entities);
        }

        public JsonResult Remove(T entity)
        {
            return this.Json(this.Service.Remove(entity));
        }

        public JsonResult RemoveRange(T[] entities)
        {
            this.Service.RemoveRange(entities);
            return this.Json(entities);
        }

        public JsonResult Update(T entity)
        {
            return this.Json(this.Service.Update(entity));
        }

        public JsonResult Find(T entity)
        {
            return this.Json(this.Service.Find(entity));
        }
    }
}