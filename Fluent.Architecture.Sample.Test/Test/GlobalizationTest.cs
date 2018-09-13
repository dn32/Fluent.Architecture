using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.TestTools;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class GlobalizationTest : FluentInternalTest
    {
        #region CONST

        private const string DescriptionEnAdd = "Description English Add";
        private const string TitleEnAdd = "Title English Add";

        private const string DescriptionEn = "Description English";
        private const string TitleEn = "Title English";

        private const string DescriptionPtBr = "Descrição de idioma portugês.";
        private const string TitlePtBr = "Título de idioma portugês.";

        private const string DescriptionPtBr2 = "Descrição de idioma portugês 2.";
        private const string TitlePtBr2 = "Título de idioma portugês 2.";

        private const string DescriptionEs = "Descripción de idioma español.";
        private const string TitleEs = "Título de idioma español.";

        #endregion

        //[Test]
        public void StressTest()
        {
            for (var i = 0; i < 100; i++)
            {
                //new Thread(() =>
                //{

                var courseController = MockUtil.GetMockController<CourseController>();
                var list = new List();
                for (var j = 0; j < 10000; j++)
                {
                    {
                        var course = InternalTestUtil.GetNewCourse();
                        course.Id = 0;
                        TestUtil.Execute<Course>(courseController, nameof(CourseController.Add), course);
                    }
                }
                //}).Start();
            }
        }

        [Test]
        public void LanguageMustBeValidOnFindFail()
        {
            var course = AddNewCurse();
            var error = TestUtil.Execute<ContextFluentValidationException>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, string.Empty });

            Assert.NotNull(error);
            Assert.AreEqual("The language should be informed.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void LanguageMustBeValidOnFindFail2()
        {
            var course = AddNewCurse();
            var error = TestUtil.Execute<ContextFluentValidationException>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, "xpto-lang" });

            Assert.NotNull(error);
            Assert.AreEqual("xpto-lang is an invalid language.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void LanguageMustBeValidOnFirstOrDefaultFail()
        {
            var error = TestUtil.Execute<ContextFluentValidationException>(this.CourseControllerInstance, nameof(CourseController.FirstOrDefaultSpec), new object[] { "xpto-lang" });

            Assert.NotNull(error);
            Assert.AreEqual("xpto-lang is an invalid language.", error.Inconsistencies.First().Message);
        }

        [Test]
        public void LanguageMustBeValidOnFirstOrDefaultOk()
        {
            AddNewCurse();

            var course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.FirstOrDefaultSpec), new object[] { "pt-BR" });

            Assert.NotNull(course);

            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }

        [Test]
        public void LanguageMustBeValidOnFindOk()
        {
            AddNewCurse();

            var course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.FirstOrDefault), new object[] { "pt-BR" });

            Assert.NotNull(course);

            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }

        [Test, Ignore("Está carregnado o banco todo")]
        public void ListOk()
        {
            var course = AddNewCurse();

            var courses = TestUtil.Execute<List<Course>>(this.CourseControllerInstance, nameof(CourseController.List), new object[] { "pt-BR" });

            Assert.NotNull(courses);

            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }

        [Test]
        public void ListPaginationOk()
        {
            var course = AddNewCurse();

            var pagination = new FluentPagination(0, 20);

            var courses = TestUtil.Execute<List<Course>>(this.CourseControllerInstance, nameof(CourseController.List), new object[] { pagination, "pt-BR" });

            Assert.NotNull(courses);

            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }

        [Test]
        public void GlobalizationTestTraduction()
        {
            var course = AddNewCurse();
            course = UpdateForCurrentLanguage(course);
            course = UpdateForAnotherLanguagePtBr(course);
            course = UpdateForAnotherLanguageEs(course);
            course = UpdateLanguagePtBr(course);

            var courseDefault = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), course);
            var coursePtBr = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.PT_BR });
            var courseEs = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.ES });
            var courseEn = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.EN_US });
            var courseAnother = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.AA_DJ });

            Assert.NotNull(courseDefault);
            Assert.NotNull(coursePtBr);
            Assert.NotNull(courseEs);
            Assert.NotNull(courseEn);

            Assert.AreEqual(DescriptionEs, courseDefault.Description);
            Assert.AreEqual(TitleEs, courseDefault.Title);
            Assert.AreEqual(Language.ES, courseAnother.Language);
            Assert.True(courseDefault.IsDefaultLanguage);

            Assert.AreEqual(DescriptionPtBr2, coursePtBr.Description);
            Assert.AreEqual(TitlePtBr2, coursePtBr.Title);
            Assert.AreEqual(Language.PT_BR, coursePtBr.Language);
            Assert.False(coursePtBr.IsDefaultLanguage);

            Assert.AreEqual(DescriptionEs, courseEs.Description);
            Assert.AreEqual(TitleEs, courseEs.Title);
            Assert.AreEqual(Language.ES, courseEs.Language);
            Assert.True(courseEs.IsDefaultLanguage);

            Assert.AreEqual(DescriptionEn, courseEn.Description);
            Assert.AreEqual(TitleEn, courseEn.Title);
            Assert.AreEqual(Language.EN_US, courseEn.Language);
            Assert.False(courseEn.IsDefaultLanguage);

            Assert.AreEqual(DescriptionEs, courseAnother.Description);
            Assert.AreEqual(TitleEs, courseAnother.Title);
            Assert.AreEqual(Language.ES, courseAnother.Language);
            Assert.True(courseAnother.IsDefaultLanguage);

            ////Remove
            TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Remove), course);
        }

        #region INTERNAL

        /// <summary>
        /// Cadastra um curso com idioma padrão inglês
        /// </summary>
        private Course AddNewCurse()
        {
            //Add en-US (default)
            var course = InternalTestUtil.GetNewCourse();
            course.Id = 0;
            course.Description = DescriptionEnAdd;
            course.Title = "Title English Add";
            course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Add), course);
            Assert.NotNull(course);
            Assert.AreNotEqual(0, course.Id);
            Assert.AreEqual(DescriptionEnAdd, course.Description);
            Assert.AreEqual(TitleEnAdd, course.Title);

            return course;
        }

        /// <summary>
        /// Atualiza o texto do idioma padrão inglês para um novo texto.
        /// </summary>
        private Course UpdateForCurrentLanguage(Course course)
        {
            //Update default language
            course.Description = DescriptionEn;
            course.Title = TitleEn;
            course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
            Assert.NotNull(course);
            Assert.AreNotEqual(0, course.Id);
            Assert.AreEqual(DescriptionEn, course.Description);
            Assert.AreEqual(TitleEn, course.Title);

            return course;
        }

        /// <summary>
        /// Atualiza o idioma para portugês, sem definir como padrão, fazendo com que dessa forma, um novo idioma seja adicionado, mantendo o antigo como padrão.
        /// </summary>
        private Course UpdateForAnotherLanguagePtBr(Course course)
        {
            //Add another language (pt-BR)
            course.Description = DescriptionPtBr;
            course.Title = TitlePtBr;
            course.IsDefaultLanguage = false;
            course.Language = Language.PT_BR;

            course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
            var coursePtBr = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.PT_BR });
            Assert.NotNull(course);
            Assert.AreNotEqual(0, course.Id);
            Assert.AreEqual(DescriptionEn, course.Description);
            Assert.AreEqual(TitleEn, course.Title);

            Assert.NotNull(coursePtBr);
            Assert.AreEqual(DescriptionPtBr, coursePtBr.Description);
            Assert.AreEqual(TitlePtBr, coursePtBr.Title);

            return course;
        }

        /// <summary>
        /// Atualiza o idioma para espenhol, definindo como padrão. Dessa forma, espanhol passa a ser o idioma padrão e o idioma anterior (inglês),
        /// passa a ser um idioma secundário, contento ainda o textos antigos.
        /// </summary>
        private Course UpdateForAnotherLanguageEs(Course course)
        {
            //Update default to es and Add es
            course.Description = DescriptionEs;
            course.Title = TitleEs;
            course.IsDefaultLanguage = true;
            course.Language = Language.ES;

            course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
            var courseEn = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Find), new object[] { course, Language.EN_US });
            Assert.NotNull(course);
            Assert.AreNotEqual(0, course.Id);
            Assert.AreEqual(DescriptionEs, course.Description);
            Assert.AreEqual(TitleEs, course.Title);

            Assert.NotNull(courseEn);
            Assert.AreEqual(DescriptionEn, courseEn.Description);
            Assert.AreEqual(TitleEn, courseEn.Title);

            return course;
        }

        /// <summary>
        /// Atualiza os textos do idioma secundário portugês, sem afetar o idioma original (espanhol) e o outro idioma secundário (inglês).
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        private Course UpdateLanguagePtBr(Course course)
        {
            //Update another language (pt-BR)
            course.Description = DescriptionPtBr2;
            course.Title = TitlePtBr2;
            course.IsDefaultLanguage = false;
            course.Language = Language.PT_BR;

            course = TestUtil.Execute<Course>(this.CourseControllerInstance, nameof(CourseController.Update), course);
            Assert.NotNull(course);
            Assert.AreNotEqual(0, course.Id);
            Assert.AreEqual(DescriptionEs, course.Description);
            Assert.AreEqual(TitleEs, course.Title);

            return course;
        }

        #endregion

    }
}
