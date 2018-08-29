using System;
using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Test.SupportElements.Mock
{
    public static class ControllerMockFactory
    {
        public static TC Create<TC>() where TC : BaseController, new()
        {
            return ControllerMockFactory.Create(typeof(TC)) as TC;
        }

        public static BaseController Create(Type controllerType)
        {
            return Activator.CreateInstance(controllerType) as BaseController;
        }
    }
}
