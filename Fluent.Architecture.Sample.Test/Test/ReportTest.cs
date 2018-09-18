
using System.Runtime.InteropServices;
using Fluent.Architecture.Sample.Test.SupportElements;
using Fluent.Architecture.Sample.Test.SupportElements.Controllers;
using Fluent.Architecture.Test;
using Fluent.Architecture.Validation;
using NUnit.Framework;

namespace Fluent.Architecture.Sample.Test.Test
{
    [TestFixture]
    [ComVisible(true)]
    public class ReportTest : FluentInternalTest
    {
        [Test]
        public void AddPropagationFullParameterIsNullFail()
        {
            var error = TestUtil.Execute<ContextFluentValidationException>(ReportControllerInstance, nameof(ReportController.GenerateError), null);
            Assert.NotNull(error);
            Assert.AreEqual("Value can not be null", error.Message);
        }
    }
}
