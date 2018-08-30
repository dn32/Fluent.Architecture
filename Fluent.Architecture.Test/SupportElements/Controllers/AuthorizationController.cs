#if NET461
using System.Collections.Generic;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Controllers
{
    public class StudentController : FluentController<Student>
    {
        public JsonResult SpecOne(FluentSpecification<Student> spec)
        {
            var student = this.Service.SpecOne(spec);
            return this.Json(student);
        }

        public JsonResult Add(Student student)
        {
            return this.Json(this.Propagate(student));
        }

        public JsonResult Update(Student student)
        {
            return this.Json(this.Propagate(student));
        }

        public JsonResult Add2(Student student)
        {
            return this.Json(this.PropagateMethod(nameof(FluentService<Student>.Add), student));
        }

        public virtual JsonResult Spec(FluentSpecification<Student> spec)
        {
            return this.Json(this.Propagate<List<Student>>(spec));
        }

        public virtual JsonResult Spec2()
        {
            return this.Json(this.PropagateMethod(nameof(FluentService<Student>.Spec), null));
        }

        public JsonResult Find(Student student)
        {
            return this.Json(this.Service.Find(student));
        }

        public JsonResult Remove(Student student)
        {
            student = this.Service.Remove(student);
            return this.Json(student);
        }
    }
}
#endif
