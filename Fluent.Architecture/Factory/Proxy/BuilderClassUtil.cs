// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Fluent.Architecture.Factory.Proxy
{
    internal static class BuilderClassUtil
    {
        internal static TypeBuilder CreateClass(Type parent, string assemblyName, string moduleName)
        {
            var assembly = new AssemblyName(assemblyName);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            return moduleBuilder.DefineType(assembly.FullName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, parent);
        }

        internal static void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }
    }
}
