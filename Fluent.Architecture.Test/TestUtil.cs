using dn32.infra.Test.Mock;
using dn32.infra.Test.Mock.ControllerMock;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using dn32.infra.dados;
using dn32.infra.nucleo.controladores;

namespace dn32.infra.Test
{
    [ComVisible(true)]
    public static class TestUtil
    {
        public static DnControladorBase GetController(Type controllerType)
        {
            return MockControllerFactory.Create(controllerType);
        }

        public static TR Execute<TC, TR>(TC controller, Func<TC, object> actionMethod) where TC : DnControladorBase
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            ResultadoPadrao<TR> result;

            try
            {
                result = actionMethod(controller) as ResultadoPadrao<TR>;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            if (result == null) { return default; }
            return JsonConvert.DeserializeObject<TR>(JsonConvert.SerializeObject(result.Dados));
        }

        public static object Execute<TC>(TC controller, Func<TC, object> actionMethod) where TC : DnControladorBase
        {
            controller.OnActionExecuting(MockActionExecutingContextFactory.Create(controller));
            object result;

            try
            {
                result = actionMethod(controller);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            controller.OnActionExecuted(MockActionExecutedContextFactory.Create(controller));
            return result;
        }
    }
}
