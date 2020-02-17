using System;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    //Todo - 001 Testar
    public static class TypeExtension
    {
        public static bool GetCustomAttributeAny<T>(this MemberInfo info, bool inherit = false) where T : Attribute =>
            info.GetCustomAttribute<T>(inherit) != null;

        public static bool GetCustomAttributeAny<T>(this ParameterInfo info, bool inherit = false) where T : Attribute =>
            info.GetCustomAttribute<T>(inherit) != null;

        public static T FluentCast<T>(this object obj, bool throwException = true)
        {
            if (obj == null) return default;

            if (obj is T value)
                return value;
            else if (throwException)
                throw new InvalidOperationException($"{obj.GetType().Name} is not a {typeof(T).Name}.");

            return default;
        }
    }
}