using System;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Service;

namespace Fluent.Architecture.Test
{
    internal class ControllerMockClassBuilder
    {
        internal static object Create(Type serviceType, TransactionalService service, TransactionInterceptor interceptor)
        {
            throw new NotImplementedException();
        }

        internal static BaseController Create(Type controllerType, TransactionInterceptor interceptor)
        {
            throw new NotImplementedException();
        }
    }
}