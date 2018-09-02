using System.Runtime.InteropServices;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class TranslactionTest : FluentInternalTest
    {
        [Test]
        public void CountSuccessTest()
        {
            var course = InternalTestUtil.GetNewCourse();
            course.Id = 0;

            //Add
            var courseAdded = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Add), course);
            Assert.NotNull(courseAdded);
            Assert.AreNotEqual(0, courseAdded.Id);

            //var course2 = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(UserController.Find), course);

            ////Remove
            //TestUtil.Execute<User>(this.CourseControllerInstance, nameof(FluentFullController<User>.Remove), course);
        }
    }

}
