// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Web;

namespace Fluent.Architecture.Test.Mock.ControllerMock
{
    public class HttpResponseBaseMock : HttpResponseBase
    {
        public override void Clear()
        {
        }

        public override int StatusCode { get; set; }

        public override bool TrySkipIisCustomErrors { get; set; }
    }
}


