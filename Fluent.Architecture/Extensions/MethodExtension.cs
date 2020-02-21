// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Linq;
using System.Reflection;

namespace dn32.infra.extensoes
{
    /// <summary>
    /// Extensão de MethodBase e MethodInfo.
    /// </summary>
    public static class MethodExtension
    {
        /// <summary>
        /// Obtém o nome amigável de um método. Exemplo Adicionar(User user)
        /// </summary>
        /// <param Nome="method">Método a ser tratado.</param>
        /// <param Nome="showParameterName">Se deseja mostrar o nome dos parâmetros. Exemplo com true: Adicionar(User user). Exemplo com false: Adicionar(User)</param>
        /// <returns>O nome amigável do método.</returns>
        public static string GetFriendlyName(this MethodBase method, bool showParameterName = false)
        {
            if (method == null) { return "null"; }
            return method.Name + (method.ContainsGenericParameters ? "<" + string.Join(", ", method.GetGenericArguments().Select(x => x.Name)) + ">" : string.Empty) +
                "(" + string.Join(", ", method.GetParameters().Select(x => x.ParameterType.GetFriendlyName(false) + (showParameterName ? " " + x.Name : string.Empty))) + ")";
        }

        // Todo2 doc
        public static object[] GetAllParameters(this MethodBase method)
        {
            return method?.GetParameters()?.Select(x => x.DefaultValue)?.ToArray();
        }

        // Todo2 doc
        public static MethodInfo GetMethodWithoutAmbiguity(this Type classType, string methodName, object[] parameters, params Type[] generics)
        {

            var methods = classType?.GetMethods()?.Where(x =>
                x.Name == methodName &&
                parameters.Length <= x.GetParameters().Length &&
                parameters.Length >= x.GetParameters().Count(y => !y.IsOptional) &&
                parameters.Length + x.GetParameters().Count(y => y.IsOptional) >= x.GetParameters().Length
            ) ?? Array.Empty<MethodInfo>();

            foreach (var method in methods)
            {
                if (method.IsGenericMethod == true && (generics == null || generics.Length == 0) ||
                    method.IsGenericMethod == false && (generics != null && generics.Length > 0))
                {
                    continue;
                }

                var currentMethod = method.IsGenericMethod && (generics?.Length > 0) ? method.MakeGenericMethod(generics) : method;
                var parametersOfMethodType = currentMethod.GetParameters().Select(x => x.ParameterType).ToList();
                var parametersListType = parameters.Select(x => x.GetType()).ToList();

                if (parameters.All(x => parametersListType.Next().Is(parametersOfMethodType.Next())))
                {
                    return currentMethod;
                }
            }

            return null;
        }

        // Todo2 doc
        public static object DnInvoke(this MethodInfo method, object entity, object[] parameters)//, params Tipo[] generics)
        {
            //method = generics == null ? method : method.MakeGenericMethod(generics);
            if (parameters == null) { parameters = Array.Empty<object>(); }
            var localParameters = method?.GetAllParameters();

            if (localParameters?.Length != parameters.Length)
            {
                throw new InvalidOperationException($"the number of method parameters {method?.Name} is different from the amount informed in the call");
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] != null)
                {
                    localParameters[i] = parameters[i];
                }
            }

            try
            {
                return method?.Invoke(entity, localParameters);
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
