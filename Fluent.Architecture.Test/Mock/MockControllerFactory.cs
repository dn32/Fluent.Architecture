// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.controladores;
using System;
using dn32.infra.nucleo.controladores;

namespace dn32.infra.Test.Mock
{
    public static class MockControllerFactory
    {
        public static TC Create<TC>() where TC : ControladorBase, new()
        {
            return Create(typeof(TC)) as TC;
        }

        public static ControladorBase Create(Type controllerType)
        {
            return Activator.CreateInstance(controllerType) as ControladorBase;
        }
    }
}
