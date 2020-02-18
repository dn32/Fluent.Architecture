// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;
using dn32.infra.nucleo.controladores;

namespace dn32.infra.Test.Mock
{
    public static class MockControllerFactory
    {
        public static TC Create<TC>() where TC : DnControladorBase, new()
        {
            return Create(typeof(TC)) as TC;
        }

        public static DnControladorBase Create(Type controllerType)
        {
            return Activator.CreateInstance(controllerType) as DnControladorBase;
        }
    }
}
