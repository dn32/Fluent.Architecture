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
            if (property.PropertyType.IsNumeric())
            {
                var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                if (min == null || max == null) { return RandomUtil.NextRandom(int.MaxValue); }

                return RandomUtil.NextRandom((min as int?).Value, (max as double?).Value);
            }

            if (property.PropertyType == typeof(string) && property.PropertyType == typeof(String))
            {
                var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                if (min == null && max == null) { return RandomUtil.NextRandom(12); }
                if (min == null) { return RandomUtil.NextRandomString(max.Value); }
                if (max == null) { return RandomUtil.NextRandomString(min.Value); }
                return RandomUtil.NextRandomString(max.Value);
            }

            return property.PropertyType.GetDefaultValue();
        }
    }
}
