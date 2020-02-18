using Microsoft.AspNetCore.Mvc.Controllers;

namespace dn32.infra.Test.Mock.Novos
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
