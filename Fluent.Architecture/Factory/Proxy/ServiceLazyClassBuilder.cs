// ReSharper disable CommentTypo
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Fluent.Architecture.Service;

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
            var type = dynamicClass.CreateType();
            return Activator.CreateInstance(type);
        }

        private static TypeBuilder CreateClass(Type parent, AssemblyName assembly)
        {

#if NET461
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
#else
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
#endif
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
            return moduleBuilder.DefineType(assembly.FullName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, parent);
        }

        private static void OverwriteProperties(TypeBuilder typeBuilder, Guid sessionId)
        {
            var serviceProperties = typeBuilder.BaseType?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(x => x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();
            if (serviceProperties == null)
            {
                return;
            };

            foreach (var property in serviceProperties)
            {
                OverwriteProperty(typeBuilder.BaseType, property, typeBuilder, sessionId);
            }
        }

        private static void OverwriteProperty(Type baseType, PropertyInfo property, TypeBuilder typeBuilder, Guid sessionId)
        {
            var metodProp = property.GetGetMethod(true);
            var propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.HasDefault, property.PropertyType, null);
            var getPropMthdBldr = typeBuilder.DefineMethod(metodProp.Name,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.Virtual |
                MethodAttributes.HideBySig,
                property.PropertyType,
                Type.EmptyTypes);

            var getIl = getPropMthdBldr.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            var methodInfo = baseType.GetMethod(nameof(BaseService.GetServiceDependency));
            methodInfo = methodInfo.MakeGenericMethod(property.PropertyType);
            getIl.Emit(OpCodes.Ldstr, sessionId.ToString());
            getIl.EmitCall(OpCodes.Callvirt, methodInfo, new[] { typeof(string) });
            //getIl.Emit(OpCodes.Callvirt, methodInfo);
            getIl.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getPropMthdBldr);
        }

        private static void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }
    }

}
