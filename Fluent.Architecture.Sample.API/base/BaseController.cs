using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Sample.API
{
    public class BaseController<T> : FluentController<T> where T : BaseEntity
    {
        //[HttpPost]
        //public object Add([FromBody] T entity)
        //{
        //    return Service.Add(entity);
        //}

        //[HttpPost]
        //public object AddRange([FromBody] T[] entities)
        //{
        //    Service.AddRange(entities);
        //    return entities;
        //}

        //[HttpPost]
        //public object Update([FromBody] T entity)
        //{
        //    return Service.Update(entity);
        //}

        //[HttpPost]
        //public object Remove([FromBody] T entity)
        //{
        //    return Service.Remove(entity);
        //}

        //[HttpPost]
        //public void RemoveRange([FromBody] T[] entities)
        //{
        //    Service.RemoveRange(entities);
        //}

        [HttpPost]
        public object Find(T entity)
        {
            return Service.Find(entity);
        }
    }
}
