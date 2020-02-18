// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.Controllers;
using System;

namespace dn32.infra.Test.Mock
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
