using Fluente.Arquitetura.Controllers;
using Fluente.Arquitetura.Exceptions;
using Fluente.Arquitetura.Factory.Proxy;
using Fluente.Arquitetura.Nucleo.Atributos;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluente.Arquitetura.Nucleo.Factory
{
    public class ControllerFactory : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var baseController = (Setup.Config.Config.GenericControllerType) ?? typeof(FluenteAPIController<>);
            var entities = Setup.GetFluenteApiEntity();

            foreach (var entity in entities)
            {
                if (entity.GetCustomAttribute<FluenteAPIControllerAttribute>(true)?.AutomaticGeneration == false) { continue; }
                if (Setup.Controllers.ContainsKey(entity)) { continue; }

                if (entity.GetCustomAttribute<FluenteJsonFormAttribute>(true)?.IsReadOnly == true)
                {
                    baseController = typeof(FluenteAPIReadOnlyController<>);
                }

                var typeName = entity.Name + "Controller";
                if (feature.Controllers.Any(t => t.Name == typeName))
                {
                    throw new IncorrectDevelopmentException($"There is a controller named {typeName}. This interferes with the creation of a generic controller with this name for the {entity.Name} entity. Consider renaming this controller or entity");
                }

                var parentClass = baseController.MakeGenericType(entity);
                var moduleName = $"FluenteDynamicModule{entity.Name}";

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
