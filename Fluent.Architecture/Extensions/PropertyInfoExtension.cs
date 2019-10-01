using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Fluent.Architecture.Core.Extensions
{
    public static class PropertyInfoExtension
    {
        public static object GetExampleValue(this PropertyInfo property)
        {
            if (property.PropertyType.GetNonNullableType().IsNumeric())
            {
                var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum ?? 0;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum ?? double.MaxValue;
                return RandomUtil.NextRandom(int.Parse(min.ToString() ?? "0"), double.Parse(max.ToString() ?? "0"));
            }

            if (property.PropertyType.GetNonNullableType() == typeof(string) && property.PropertyType.GetNonNullableType() == typeof(String))
            {
                //var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length ?? 0;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? double.MaxValue;
                max = max > 64 ? 64 : max;
                return RandomUtil.NextRandomString(max);
            }

            return property.PropertyType.GetFluentDefaultValue();
        }
    }
}
