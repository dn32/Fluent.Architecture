using dn32.infra.controladores;
using dn32.infra.Exceptions;
using dn32.infra.Factory.Proxy;
using dn32.infra.nucleo.atributos;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dn32.infra.Nucleo.Factory
{
    public class ControllerFactory : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var baseController = (Setup.Config.Config.GenericControllerType) ?? typeof(DnApi<>);
            var entities = Setup.GetDnApiEntity();

            foreach (var entity in entities)
            {
                if (entity.GetCustomAttribute<DnControladorApiAtributo>(true)?.GerarAutomaticamente == false) { continue; }
                if (Setup.Controllers.ContainsKey(entity)) { continue; }

                if (entity.GetCustomAttribute<DnFormularioJsonAtributo>(true)?.EhSomenteLeitura == true)
                {
                    baseController = typeof(DnApiReadOnly<>);
                }

                var typeName = entity.Name + "Controller";
                if (feature.Controllers.Any(t => t.Name == typeName))
                {
                    throw new IncorrectDevelopmentException($"There is a controller named {typeName}. This interferes with the creation of a generic controller with this Nome for the {entity.Name} entity. Consider renaming this controller or entity");
                }

                var parentClass = baseController.MakeGenericType(entity);
                var moduleName = $"DnDynamicModule{entity.Name}";

                var dynamicClass = BuilderClassUtil.CreateClass(parentClass, typeName, moduleName);
                BuilderClassUtil.CreateConstructor(dynamicClass);
                var type = dynamicClass.CreateType() ?? throw new InvalidOperationException($"ControllerFactory not create {typeName}");

                var controllerType = type.GetTypeInfo();
                feature.Controllers.Add(controllerType);
                Setup.Controllers.Add(entity, controllerType);
            }
        }
    }
}
