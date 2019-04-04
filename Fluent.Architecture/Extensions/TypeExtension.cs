// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Factory.Interface;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Extensão de Type.
    /// </summary>
    public static class TypeExtension
    {
        private static readonly ConcurrentDictionary<Type, object> TypeDefaults = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Obtem o valor padrão de um tipo.
        /// Muito útil para preencher construtores de classes por reflexão.
        /// </summary>
        /// <param name="type">O tipo a ser avaliado.</param>
        /// <returns>O valor padrão do tipo.</returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance) : null;
        }

        //Todo doc
        public static TX GetDefaultValue<TX>()
        {
            return (TX)typeof(TX).GetDefaultValue();
        }

        //Todo doc
        public static TX Next<TX>(this List<TX> list)
        {
            if (list.Count == 0)
            {
                return GetDefaultValue<TX>();
            }

            var el = list.First();
            list.RemoveAt(0);
            return el;
        }

        //Todo doc
        public static bool Is(this Type t1, Type t2)
        {
            return t1.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t2) ||
                   t2.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t1) ||
                   t1 == t2 || t1.IsSubclassOf(t2) || t2.IsAssignableFrom(t1) || t2.IsSubclassOf(t1) || t1.IsAssignableFrom(t2);
        }

        // Todo - Documentar
        public static object[] GetConstructorParameters(this Type classType)
        {
            var parameters = classType.GetConstructors().First().GetParameters();
            return parameters.Select(x => x.ParameterType.GetDefaultValue()).ToArray();
        }

        /// <summary>
        /// Obtem o nome amigável de um tipo. Exemplo: FluentSelectSpecification FluentEntity
        /// </summary>
        /// <param name="type">O tipo a ser tratado.</param>
        /// <param name="useGenericT">Se deve indicar os tipos genéricos como T. Exemplo com true: FluentSelectSpecification T, T. Exemplo com false: FluentSelectSpecification FluentEntity, TO </param>
        /// <returns>O nome amigável do tipo.</returns>
        public static string GetFriendlyName(this Type type, bool useGenericT = true)
        {
            var friendlyName = type.Name;
            if (!type.IsGenericType) return friendlyName;
            var iBacktick = friendlyName.IndexOf('`');
            if (iBacktick > 0)
            {
                friendlyName = friendlyName.Remove(iBacktick);
            }

            friendlyName += "<";
            var typeParameters = type.GetGenericArguments();
            for (var i = 0; i < typeParameters.Length; ++i)
            {
                var typeParamName = GetFriendlyName(typeParameters[i], useGenericT);
                typeParamName = useGenericT ? "T" : typeParamName;
                friendlyName += (i == 0 ? typeParamName : ", " + typeParamName);
            }

            friendlyName += ">";
            return friendlyName;
        }

        public static Type GetSpecializedService(this Type serviceType)
        {
            if (serviceType.Name == "FluentDynamicProxy")
            {
                serviceType = serviceType.BaseType;
            }

            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Services.TryGetValue(entityType, out serviceType))
                {
                    if (entityType.IsSubclassOf(typeof(FluentGlobalizedEntity)))
                    {
                        serviceType = typeof(FluentGlobalizedService<>).MakeGenericType(entityType);
                    }
                    else
                    {
                        serviceType = typeof(FluentService<>).MakeGenericType(entityType);
                    }
                }
            }

            return serviceType;
        }
    }
}