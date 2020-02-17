// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluente.Arquitetura.Controllers;
using System;

namespace Fluente.Arquitetura.Test.Mock
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
