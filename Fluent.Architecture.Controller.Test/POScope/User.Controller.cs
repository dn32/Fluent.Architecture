// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controllers;
using NUnit.Framework;

namespace Fluent.Architecture.Controller.Test.POScope
{
    public class UserController : FluentAPIController<User>
    {
        public DefaultResult InternalCheck()
        {
            //Tests
            Assert.IsNotNull(SessionRequestId);
            Assert.IsNotNull(ServiceHttpContext);
            Assert.IsNotNull(ServiceUser);
            Assert.IsNotNull(User);
            return Result(true);
        }
    }
}


