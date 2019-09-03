// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
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
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            return type.IsValueType ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance) : null;
        }

        //Todo2 doc
        public static TX GetDefaultValue<TX>()
        {
            return (TX)typeof(TX).GetDefaultValue();
        }

        //Todo2 doc
        public static TX Next<TX>(this List<TX> list)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }

            if (list.Count == 0)
            {
                return GetDefaultValue<TX>();
            }

            var el = list.First();
            list.RemoveAt(0);
            return el;
        }

        //Todo2 doc
        public static bool Is(this Type t1, Type t2)
        {
            if (t1 == null) { throw new ArgumentNullException(nameof(t1)); }
            if (t2 == null) { throw new ArgumentNullException(nameof(t2)); }

            return t1.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t2) ||
                   t2.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t1) ||
                   t1 == t2 || t1.IsSubclassOf(t2) || t2.IsAssignableFrom(t1) || t2.IsSubclassOf(t1) || t1.IsAssignableFrom(t2);
        }

        public static bool IsNullableEnum(this Type t)
        {
            if (t?.IsEnum == true) { return true; }
            var u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }

        public static Type GetTypeByNullType(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);
            return u ?? t;
        }

        public static bool IsNumeric(this Type type)
        {
            if (type.IsNullableEnum())
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        // Todo - Documentar
        public static object[] GetConstructorParameters(this Type classType)
        {
            var parameters = classType?.GetConstructors()?.First()?.GetParameters();
            return parameters?.Select(x => x?.ParameterType?.GetDefaultValue())?.ToArray();
        }

        /// <summary>
        /// Obtem o nome amigável de um tipo. Exemplo: FluentSelectSpecification FluentEntity
        /// </summary>
        /// <param name="type">O tipo a ser tratado.</param>
        /// <param name="useGenericT">Se deve indicar os tipos genéricos como T. Exemplo com true: FluentSelectSpecification T, T. Exemplo com false: FluentSelectSpecification FluentEntity, TO </param>
        /// <returns>O nome amigável do tipo.</returns>
        public static string GetFriendlyName(this Type type, bool useGenericT = true)
        {
            if (type == null) { return "null"; }
            var friendlyName = type.Name;
            if (!type.IsGenericType) { return friendlyName; }
            var iBacktick = friendlyName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
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
            if (serviceType?.Name == "FluentDynamicProxy")
            {
                serviceType = serviceType.BaseType;
            }

            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Services.TryGetValue(entityType, out serviceType))
                {
                    var type = (Setup.Config.Config.GenericServiceType) ?? typeof(FluentService<>);
                    serviceType = type.MakeGenericType(entityType);
                }
            }

            return serviceType;
        }
    }
}