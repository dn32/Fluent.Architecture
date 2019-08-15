//// -----------------------------------------------------------------------
//// <copyright company="Fluent System">
////     Copyright © Fluent System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using System.Runtime.InteropServices;
//using Fluent.Architecture.Sample.Test.SupportElements;
//using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
//using Fluent.Architecture.Test;
//using Fluent.Architecture.Validation;
//using NUnit.Framework;

//namespace Fluent.Architecture.Sample.Test.Test
//{
//    [TestFixture]
//    [ComVisible(true)]
//    internal class CustomValidationTest : FluentInternalTest
//    {
//        [Test]
//        public void AddValidationParameterIsNullFail()
//        {
//            var error = Assert.Throws<ContextFluentValidationException>(() =>
//            {
//                 TestUtil.Execute<ReportController, object>(ReportControllerInstance,(ReportController controller) => controller.GenerateError());
//            });

//            Assert.NotNull(error);
//            Assert.AreEqual("* Value Can Not Be Null", error.Message);
//        }
//    }
//}
