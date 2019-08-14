// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class StudentController : FluentController<Student>
    {
        public Student Add(Student student)
        {
            return Service.Add(student);
        }

        public Student Update(Student student)
        {
            return Service.Update(student);
        }

        //public JsonResult Add2(Student student)
        //{
        //    return this.Json(this.PropagateMethod(nameof(FluentService<Student>.Add), student));
        //}

        //public virtual JsonResult List(string name)
        //{
        //    var spec = CreateSpec<StudentByNameSpec>().DefineParams(name);
        //    return this.Json(this.Propagate<List<Student>>(spec));
        //}

        //public virtual JsonResult SpecOn(string name)
        //{
        //    var spec = CreateSpec<StudentByNameSpec>().DefineParams(name);
        //    return this.Json(this.Propagate<List<Student>>(spec));
        //}

        //public virtual JsonResult List2()
        //{
        //    return this.Json(this.PropagateMethod(nameof(FluentService<Student>.List), null));
        //}

        public Student Find(Student student)
        {
            return Service.Find(student);
        }

        public Student Remove(Student student)
        {
            return Service.Remove(student);
        }
    }
}

