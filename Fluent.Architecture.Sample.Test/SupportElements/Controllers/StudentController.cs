// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Microsoft.SqlServer.Server;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class StudentController : FluentController<Student>
    {
        public JsonResult Add(Student student)
        {
            return this.Json(Service.Add(student));
        }

        public JsonResult Update(Student student)
        {
            return this.Json(Service.Update(student));
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

