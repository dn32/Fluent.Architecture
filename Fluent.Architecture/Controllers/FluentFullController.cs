using System.Web.Mvc;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Controllers
{
    //Todo item novo. Documentar
    public class FluentFullController<T> : FluentController<T> where T : BaseEntity, new()
    {
        public JsonResult Add(T entity)
        {
            return Json(Service.Add(entity));
        }

        public JsonResult AddRange(T[] entities)
        {
            Propagate(entities as object);
            return Json(entities);
        }

        public JsonResult Remove(T entity)
        {
            return Json(Service.Remove(entity));
        }

        public JsonResult RemoveRange(T[] entities)
        {
            Service.RemoveRange(entities);
            return Json(entities);
        }

        public JsonResult Update(T entity)
        {
            return Json(Service.Update(entity));
        }

        public JsonResult Find(T entity)
        {
            return Json(Service.Find(entity));
        }
    }
}