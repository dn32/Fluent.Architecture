// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Fluent.Architecture.Factory.Proxy
{
    /// <summary>
    /// Classe interna.
    /// Responsável pela criação do proxi dos serviços de injeção de dependência.
    /// </summary>
    internal class ServiceLazyClassBuilder
    {
        private const string ModuleName = "ServiceModule";
        private const string AssemblyName = "FluentDynamicProxy";

        internal static object CreateObject(Type parent, Guid sessionId)
        {
            var dynamicClass = BuilderClassUtil.CreateClass(parent, AssemblyName, ModuleName);
            BuilderClassUtil.CreateConstructor(dynamicClass);
            OverwriteProperties(dynamicClass, sessionId);
            var type = dynamicClass.CreateType();
            return Activator.CreateInstance(type);
        }


        private static void OverwriteProperties(TypeBuilder typeBuilder, Guid sessionId)
        {
            var serviceProperties = typeBuilder.BaseType?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(x => x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

            //if (serviceProperties == null)
            //{
            //    throw new InvalidOperationException("typeBuilder not contains a BaseType");
            //}

            foreach (var property in serviceProperties)
            {
                OverwriteProperty(typeBuilder.BaseType, property, typeBuilder, sessionId);
            }
        }

        private static void OverwriteProperty(Type baseType, PropertyInfo property, TypeBuilder typeBuilder, Guid sessionId)
        {
            var method = property.GetGetMethod(true);
            var propertyBuilder = typeBuilder.DefineProperty(
                property.Name,
                PropertyAttributes.HasDefault,
                property.PropertyType,
                null);
            var getProp = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual
                | MethodAttributes.HideBySig,
                property.PropertyType,
                Type.EmptyTypes);

            var getIl = getProp.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            var methodInfo = baseType.GetMethod(nameof(BaseService.GetServiceDependency));
            //if (methodInfo == null)
            //{
            //    throw new MethodNotFoundException(nameof(BaseService.GetServiceDependency));
            //}

            methodInfo = methodInfo.MakeGenericMethod(property.PropertyType);
            getIl.Emit(OpCodes.Ldstr, sessionId.ToString());
            getIl.EmitCall(OpCodes.Callvirt, methodInfo, new[] { typeof(string) });

            // getIl.Emit(OpCodes.Callvirt, methodInfo);
            getIl.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getProp);
        }
    }
}
