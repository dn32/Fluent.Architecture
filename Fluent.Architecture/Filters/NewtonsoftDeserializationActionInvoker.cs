#if NET461

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Fluent.Architecture.Filters
{
    // Todo - Implementar pra net core
    // Implementar essa classe na dll do seu projeto, pois se não, não será possível interceptar a action de dentro da arquitetura
    public abstract class NewtonsoftDeserializationActionInvoker : ControllerActionInvoker
    {
        private string RequestBody(ControllerContext controllerContext)
        {
            var bodyStream = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            return bodyText;
        }

        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            if (actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NewtonsoftDeserializationAttribute), true).Any() || actionDescriptor.GetCustomAttributes(typeof(NewtonsoftDeserializationAttribute), true).Any())
            {
                if (parameters.Count != 1)
                {
                    throw new IncorrectDevelopmentException($"Actions decorated with {nameof(NewtonsoftDeserializationAttribute)} must have only one parameter.{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}");
                }

                var parameter = parameters.First();

                parameters[parameter.Key] = JsonConvert.DeserializeObject(RequestBody(controllerContext), parameter.Value.GetType());
            }

            return base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
        }
    }
}

#endif