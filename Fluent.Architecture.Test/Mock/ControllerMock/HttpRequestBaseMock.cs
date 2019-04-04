// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Web;

namespace Fluent.Architecture.Test.Mock.ControllerMock
{
    public sealed class HttpRequestBaseMock : HttpRequestBase
    {
        public override Uri Url { get; }

        public override string ApplicationPath { get; }

        public override NameValueCollection ServerVariables { get; }

        public override NameValueCollection Params { get; }

        public HttpRequestBaseMock()
        {
            this.Url = new Uri("http://localhost");
            this.ApplicationPath = "/";// AppDomain.CurrentDomain.BaseDirectory;
            this.Params = new NameValueCollection();

            this.ServerVariables = new NameValueCollection
                {
                    { "HTTP_HOST", "localhost" }
                };
        }
    }
}

