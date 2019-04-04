// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Test.Mock
{
    public static class ControllerMockFactory
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
