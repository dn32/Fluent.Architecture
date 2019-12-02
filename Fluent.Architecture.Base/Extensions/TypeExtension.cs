// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    public static class TypeExtension
    {
        public static bool GetCustomAttributeAny<T>(this MemberInfo info, bool inherit = false) where T : Attribute
        {
            return info.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this ParameterInfo info, bool inherit = false) where T : Attribute
        {
            return info.GetCustomAttribute<T>(inherit) != null;
        }

        public static T FluentCast<T>(this object obj, bool throwException = true)
        {
            if (obj == null)
            {
                return default;
            }

            if (obj is T value)
            {
                return value;
            }
            else if (throwException)
            {
                throw new InvalidOperationException($"{obj.GetType().Name} is not a {typeof(T).Name}.");
            }

            return default;
        }
    }
}