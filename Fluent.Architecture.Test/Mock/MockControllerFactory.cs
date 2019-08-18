// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controllers;
using System;

namespace Fluent.Architecture.Test.Mock
{
    public static class MockControllerFactory
    {
        public static TC Create<TC>() where TC : BaseController, new()
        {
            return Create(typeof(TC)) as TC;
        }

        public static BaseController Create(Type controllerType)
        {
            return Activator.CreateInstance(controllerType) as BaseController;
        }
    }
}
