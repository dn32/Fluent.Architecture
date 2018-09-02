//using System.Data.Entity.Core.Common.CommandTrees;
//using System.Web.Mvc;
//using Fluent.Architecture.Model;
//using Fluent.Architecture.Sample.Test.SupportElements.Model;

//namespace Fluent.Architecture.Controllers
//{
//    // Todo item novo. Documentar
//    public class FluentFullController<T> : FluentController<T>
//        where T : BaseEntity, new()
//    {
//        public JsonResult Add(T entity)
//        {
//            entity = this.Service.Add(entity);
//            return this.Json(entity);
//        }

//        public JsonResult AddRange(T[] entities, Language language = null)
//        {
//            this.Propagate(new object[] { entities, language });
//            return this.Json(entities);
//        }

//        public JsonResult Remove(T entity)
//        {
//            return this.Json(this.Service.Remove(entity));
//        }

//        public JsonResult RemoveRange(T[] entities)
//        {
//            this.Service.RemoveRange(entities);
//            return this.Json(entities);
//        }

//        public JsonResult Update(T entity)
//        {
//            return this.Json(this.Service.Update(entity));
//        }

//        public JsonResult Find(T entity)
//        {
//            return this.Json(this.Service.Find(entity));
//        }

//        public JsonResult Find(T entity, string language)
//        {
//            var entityFinded = Propagate(new object[] { entity, language });
//            return this.Json(entityFinded);
//        }
//    }
//}