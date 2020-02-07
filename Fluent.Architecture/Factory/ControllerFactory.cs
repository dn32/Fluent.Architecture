using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory.Proxy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.Factory
{
    public class ControllerFactory : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var baseController = (Setup.Config.Config.GenericControllerType) ?? typeof(FluentAPIController<>);
            var entities = Setup.GetFluentApiEntity();

            foreach (var entity in entities)
            {
                if (entity.GetCustomAttribute<FluentAPIControllerAttribute>(true)?.AutomaticGeneration == false) { continue; }
                if (Setup.Controllers.ContainsKey(entity)) { continue; }

                if (entity.GetCustomAttribute<FluentJsonFormAttribute>(true)?.IsReadOnly == true)
                {
                    baseController = typeof(FluentAPIReadOnlyController<>);
                }

                var typeName = entity.Name + "Controller";
                if (feature.Controllers.Any(t => t.Name == typeName))
                {
                    throw new IncorrectDevelopmentException($"There is a controller named {typeName}. This interferes with the creation of a generic controller with this name for the {entity.Name} entity. Consider renaming this controller or entity");
                }

                var parentClass = baseController.MakeGenericType(entity);
                var moduleName = $"FluentDynamicModule{entity.Name}";

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
