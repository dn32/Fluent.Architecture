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
    public class GlobalizationTest : FluentInternalTest
    {
        [Test]
        public void GlobalizationTestTraduction()
        {
            var course = InternalTestUtil.GetNewCourse();
            course.Id = 0;

            {
                //Add en-US (default)
                course.Description = "Description English Add";
                course.Title = "Title English Add";
                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Add), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Description English Add", course.Description);
            }

            {
                //Update default language
                course.Description = "Description English";
                course.Title = "Title English";
                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Description English", course.Description);
                Assert.AreEqual("Title English", course.Title);
            }

            {
                //Add another language (pt-BR)
                course.Description = "Descrição de idioma portugês.";
                course.Title = "Título de idioma portugês.";
                course.IsDefaultLanguage = false;
                course.Language = Language.PT_BR;

                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Description English", course.Description);
                Assert.AreEqual("Title English", course.Title);
            }

            {
                //Update default to es and Add es
                course.Description = "Descripción de idioma español.";
                course.Title = "Título de idioma español.";
                course.IsDefaultLanguage = true;
                course.Language = Language.ES;

                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Descripción de idioma español.", course.Description);
                Assert.AreEqual("Título de idioma español.", course.Title);
            }


            {
                //Update another language (pt-BR)
                course.Description = "Descrição de idioma portugês 2.";
                course.Title = "Título de idioma portugês 2.";
                course.IsDefaultLanguage = false;
                course.Language = Language.PT_BR;

                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Descripción de idioma español.", course.Description);
                Assert.AreEqual("Título de idioma español.", course.Title);
            }

            {
                // Find in pt-BR
                course.Language = Language.PT_BR;
                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.PT_BR });
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Descrição de idioma portugês 2.", course.Description);
                Assert.AreEqual("Título de idioma portugês 2.", course.Title);
            }
            {
                // Find default
                course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), course);
                Assert.NotNull(course);
                Assert.AreNotEqual(0, course.Id);
                Assert.AreEqual("Descripción de idioma español.", course.Description);
                Assert.AreEqual("Título de idioma español.", course.Title);
            }

            //var course2 = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(UserController.Find), course);

            ////Remove
            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }
    }

}
