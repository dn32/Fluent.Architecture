using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Nucleo.Atributos;
using Fluente.Arquitetura.Nucleo.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using dn32.infra.extensoes;

namespace Fluente.Arquitetura.Nucleo.Extensoes
{
    public static class PropertyInfoExtension
    {
        public static object GetExampleValue(this PropertyInfo property, int max_ = 0)
        {
            if (property.PropertyType.GetNonNullableType().IsNumeric())
            {
                var min = property.GetCustomAttribute<FluenteJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                var max = property.GetCustomAttribute<FluenteJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                if (min == null || max == null) { return RandomUtil.NextRandom(int.MaxValue); }

                if (max.ToString() == "0" && property.PropertyType.IsNumeric())
                {
                    max = property.PropertyType.GetMaxValueOfNumber().FluenteCast<double>();
                }

                return RandomUtil.NextRandom((int)min, (double)max);
            }

            if (property.PropertyType.GetNonNullableType() == typeof(string) && property.PropertyType.GetNonNullableType() == typeof(String))
            {
                var min = property.GetCustomAttribute<FluenteJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                var max = max_ == 0 ? (property.GetCustomAttribute<FluenteJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length) : max_;
                max ??= 64;
                min ??= 0;
                if (min == null) { return RandomUtil.NextRandomString(max.Value); }
                if (max == null) { return RandomUtil.NextRandomString(min.Value); }

                return RandomUtil.NextRandomString(max.Value);
            }

            return property.PropertyType.GetFluenteDefaultValue();
        }
    }
}
