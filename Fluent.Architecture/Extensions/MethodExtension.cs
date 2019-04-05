// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Extensão de MethodBase e MethodInfo.
    /// </summary>
    public static class MethodExtension
    {
        /// <summary>
        /// Obtém o nome amigável de um método. Exemplo Add(User user)
        /// </summary>
        /// <param name="method">Método a ser tratado.</param>
        /// <param name="showParameterName">Se deseja mostrar o nome dos parâmetros. Exemplo com true: Add(User user). Exemplo com false: Add(User)</param>
        /// <returns>O nome amigável do método.</returns>
        public static string GetFriendlyName(this MethodBase method, bool showParameterName = false)
        {
            return method.Name + (method.ContainsGenericParameters ? "<" + string.Join(", ", method.GetGenericArguments().Select(x => x.Name)) + ">" : string.Empty) +
                "(" + string.Join(", ", method.GetParameters().Select(x => x.ParameterType.GetFriendlyName(false) + (showParameterName ? " " + x.Name : string.Empty))) + ")";
        }

        // Todo doc
        public static object[] GetAllParameters(this MethodBase method)
        {
            return method.GetParameters().Select(x => x.DefaultValue).ToArray();
        }

        // Todo doc
        public static MethodInfo GetMethodWithoutAmbiguity(this Type classType, string methodName, object[] parameters, params Type[] generics)
        {

            var methods = classType.GetMethods().Where(x =>
                x.Name == methodName &&
                parameters.Length <= x.GetParameters().Length &&
                parameters.Length >= x.GetParameters().Count(y => !y.IsOptional) &&
                parameters.Length + x.GetParameters().Count(y => y.IsOptional) >= x.GetParameters().Length
            );

            foreach (var method in methods)
            {
                if (method.IsGenericMethod == true && (generics == null || generics.Length == 0) ||
                    method.IsGenericMethod == false && (generics != null && generics.Length > 0))
                {
                    continue;
                }

                var currentMethod = method.IsGenericMethod && (generics == null || generics.Length == 0) ? method.MakeGenericMethod(generics) : method;
                var parametersOfMethodType = currentMethod.GetParameters().Select(x => x.ParameterType).ToList();
                var parametersListType = parameters.Select(x => x.GetType()).ToList();

                if (parameters.All(x => parametersListType.Next().Is(parametersOfMethodType.Next())))
                {
                    return currentMethod;
                }
            }

            return null;
        }

        // Todo doc
        public static object FluentInvoke(this MethodInfo method, object entity, object[] parameters)//, params Type[] generics)
        {
            //method = generics == null ? method : method.MakeGenericMethod(generics);

            var localParameters = method.GetAllParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] != null)
                {
                    localParameters[i] = parameters[i];
                }
            }

            try
            {
                return method.Invoke(entity, localParameters);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                throw ex.InnerException ?? throw ex;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
