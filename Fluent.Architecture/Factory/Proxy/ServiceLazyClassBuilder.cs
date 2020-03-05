// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.nucleo.servicos;
using dn32.infra.servicos;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace dn32.infra.Factory.Proxy
{
    /// <summary>
    /// Classe interna.
    /// Responsável pela criação do proxi dos serviços de injeção de dependência.
    /// </summary>
    internal class ServiceLazyClassBuilder
    {
        private const string ModuleName = "ServiceModule";
        private const string AssemblyName = "DnDynamicProxy";

        internal static object CreateObject(Type parent, Guid sessionId)
        {
            var dynamicClass = BuilderClassUtil.CreateClass(parent, AssemblyName, ModuleName);
            BuilderClassUtil.CreateConstructor(dynamicClass);
            OverwriteProperties(dynamicClass, sessionId);
            var type = dynamicClass.CreateType() ?? throw new InvalidOperationException($"ServiceLazyClassBuilder not create {parent.Name}");
            return Activator.CreateInstance(type);
        }


        private static void OverwriteProperties(TypeBuilder typeBuilder, Guid sessionId)
        {
            var serviceProperties = typeBuilder.BaseType?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(x => x.PropertyType.IsSubclassOf(typeof(DnServicoBase))).ToList();

            if (serviceProperties == null)
            {
                throw new InvalidOperationException("typeBuilder not contains a BaseType");
            }

            foreach (var property in serviceProperties)
            {
                OverwriteProperty(typeBuilder.BaseType, property, typeBuilder, sessionId);
            }
        }

        private static void OverwriteProperty(Type baseType, PropertyInfo property, TypeBuilder typeBuilder, Guid sessionId)
        {
            var method = property.GetGetMethod(true);
            if (method == null) { throw new InvalidOperationException($"Get method of {property.Name} property not found"); }
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
            var methodInfo = baseType.GetMethod(nameof(DnServicoBase.ObterDependenciaDeServico));
            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Method not found '{nameof(DnServicoBase.ObterDependenciaDeServico)}'");
            }

            methodInfo = methodInfo.MakeGenericMethod(property.PropertyType);
            getIl.Emit(OpCodes.Ldstr, sessionId.ToString());
            getIl.EmitCall(OpCodes.Callvirt, methodInfo, new[] { typeof(string) });

            // getIl.Emit(OpCodes.Callvirt, methodInfo);
            getIl.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getProp);
        }
    }
}
