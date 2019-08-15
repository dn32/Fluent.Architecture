// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controller.Test.Model;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Controller.Test.Services
{
    public class LocalTestService : FluentService<TestEntity>
    {
        public void SetUserSessionForTest(UserSessionRequest sessionRequest)
        {
            SetUserSession(sessionRequest);
        }
    }
}