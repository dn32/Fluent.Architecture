using Microsoft.AspNetCore.Mvc.Controllers;

namespace Fluent.Architecture.Test.Mock.Novos
{
    public static class MockControllerActionDescriptorFactory
    {
        public static ControllerActionDescriptor Create(string controllerName = null, string actionName = null)
        {
            var desc = new ControllerActionDescriptor
            {
                ControllerName = controllerName,
                ActionName = actionName
            };

            return desc;
        }
    }
}
