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
            var assembly = new AssemblyName(AssemblyName);
            var dynamicClass = CreateClass(parent, assembly);
            CreateConstructor(dynamicClass);
            OverwriteProperties(dynamicClass, sessionId);
            var type = dynamicClass.CreateType() ?? throw new InvalidOperationException($"Building failed {dynamicClass.Name}");
            return Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Building failed {type.Name}");
        }

        private static TypeBuilder CreateClass(Type parent, AssemblyName assembly)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
            return moduleBuilder.DefineType(assembly.FullName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, parent);
        }

        private static void OverwriteProperties(TypeBuilder typeBuilder, Guid sessionId)
        {
            if (typeBuilder.BaseType == null)
            {
                throw new InvalidOperationException("typeBuilder not contains a BaseType");
            }

            var serviceProperties = typeBuilder.BaseType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(x => x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();
                     
            foreach (var property in serviceProperties)
            {
                OverwriteProperty(typeBuilder.BaseType, property, typeBuilder, sessionId);
            }
        }

        private static void OverwriteProperty(Type baseType, PropertyInfo property, TypeBuilder typeBuilder, Guid sessionId)
        {
            var method = property.GetGetMethod(true);
            if(method == null) { throw new InvalidOperationException($"Property {property.Name} of class {baseType.Name} should have a get method"); }
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
            if (methodInfo == null)
            {
                throw new InvalidCastException($"Method not found {nameof(BaseService.GetServiceDependency)}");
            }

            methodInfo = methodInfo.MakeGenericMethod(property.PropertyType);
            getIl.Emit(OpCodes.Ldstr, sessionId.ToString());
            getIl.EmitCall(OpCodes.Callvirt, methodInfo, new[] { typeof(string) });

            // getIl.Emit(OpCodes.Callvirt, methodInfo);
            getIl.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getProp);
        }

        private static void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }
    }

}
