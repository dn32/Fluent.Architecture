#if NET461
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Service;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Controllers
{
    internal class StudentController : FluentController<Student>
    {
        public JsonResult Add(Student student)
        {
            return Json(Propagate(student));
        }

        public JsonResult Add2(Student student)
        {
            return Json(Propagate(nameof(FluentService<Student>.Add), student));
        }

        public virtual JsonResult Spec(FluentSpecification<Student> spec)
        {
            return Json(Propagate(nameof(FluentService<Student>.Spec), spec, null));
        }

        public virtual JsonResult Spec2()
        {
            return Json(Propagate(nameof(FluentService<Student>.Spec), null));
        }

        public JsonResult Find(Student student)
        {
            return Json(Service.Find(student));
        }

        public JsonResult Remove(Student student)
        {
            student = Service.Remove(student);
            return Json(student);
        }
    }
}
#endif
