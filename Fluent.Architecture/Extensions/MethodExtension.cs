// ReSharper disable CommentTypo
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
            return method.Name + (method.ContainsGenericParameters ? "<" + string.Join(", ", method.GetGenericArguments().Select(x => x.Name)) + ">" : "") +
                "(" + string.Join(", ", method.GetParameters().Select(x => x.ParameterType.GetFriendlyName(false) + (showParameterName ? " " + x.Name : ""))) + ")";
        }

        //Todo doc
        public static object[] GetAllParameters(this MethodBase method)
        {
            return method.GetParameters().Select(x => x.DefaultValue).ToArray();
        }
    }
}
