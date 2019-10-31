// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Services;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Fluent.Architecture.Attributes;
using System.Threading.Tasks;
using Fluent.Architecture.Core.Util;

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
        public static object GetFluentDefaultValue(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            type = type.GetNonNullableType();
            return type.IsValueType ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance) : null;
        }

        public static bool FluentEquals(this object value1, object value2)
        {
            if (value1 == null && value2 == null) { return true; }
            return value1?.ToString() == value2?.ToString();
        }

        //Todo2 doc
        public static TX GetDefaultValue<TX>()
        {
            return (TX)typeof(TX).GetFluentDefaultValue();
        }

        public static bool IsKey(this MemberInfo info)
        {
            return info.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || info.GetCustomAttributeAny<KeyAttribute>(true);
        }

        public static bool GetCustomAttributeAny<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this PropertyInfo property, bool inherit = false) where T : Attribute
        {
            return property.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this TypeInfo typeInfo, bool inherit = false) where T : Attribute
        {
            return typeInfo.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this MethodInfo methodInfo, bool inherit = false) where T : Attribute
        {
            return methodInfo.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this MemberInfo methodInfo, bool inherit = false) where T : Attribute
        {
            return methodInfo.GetCustomAttribute<T>(inherit) != null;
        }

        public static bool GetCustomAttributeAny<T>(this ParameterInfo methodInfo, bool inherit = false) where T : Attribute
        {
            return methodInfo.GetCustomAttribute<T>(inherit) != null;
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
                   t1 == t2 || t1.IsSubclassOf(t2) || t2.IsAssignableFrom(t1);
        }

        public static bool IsOrIsReverse(this Type t1, Type t2)
        {
            if (t1 == null) { throw new ArgumentNullException(nameof(t1)); }
            if (t2 == null) { throw new ArgumentNullException(nameof(t2)); }

            return t1.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t2) ||
                   t2.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t1) ||
                   t1 == t2 || t1.IsSubclassOf(t2) || t2.IsAssignableFrom(t1) || t2.IsSubclassOf(t1) || t1.IsAssignableFrom(t2);
        }

        public static object GetPrimitiveExampleValue(this Type type)
        {
            type = type.GetNonNullableType();

            if (type.IsNumeric()) { return RandomUtil.NextRandom(99); }
            if (type == typeof(DateTime)) { return DateTime.Now; }
            if (type == typeof(string) && type == typeof(String)) { return RandomUtil.NextRandomString(6); }

            return type.GetFluentDefaultValue();
        }

        public static string GetExampleValueString(this Type type)
        {
            var obj = type.GetExampleValue();
            if (type.IsPrimitiveOrPrimitiveNulable()) { return obj.ToString(); }
            return obj.ToFluentJson(Formatting.Indented);
        }

        public static object GetExampleValue(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            object obj;

            if (type.IsPrimitiveOrPrimitiveNulable()) { return type.GetPrimitiveExampleValue(); }

            try
            {
                obj = Activator.CreateInstance(type);
            }
            catch (MissingMethodException)
            {

                return type.GetFluentDefaultValue();
            }

            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsNullableEnum())
                {
                    var firstEnum = Enum.GetValues(property.PropertyType.GetTypeByNullType()).GetValue(1);
                    property.SetValue(obj, firstEnum);
                }
                else
                {
                    var value = property.GetExampleValue();
                    if (value != null)
                    {
                        var newValue = Convert.ChangeType(value, property.PropertyType.GetNonNullableType(), CultureInfo.InvariantCulture);
                        if (property.SetMethod != null)
                        {
                            property.SetValue(obj, newValue);
                        }
                    }
                }
            }

            return obj;
        }

        public static bool IsOfNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type GetNonNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static FieldInfo[] GetEnumFields(this Type type)
        {
            var nullValue = Nullable.GetUnderlyingType(type);
            type = nullValue ?? type;
            return type.GetFields();
        }

        public static bool IsPrimitive(this Type type)
        {
            if (type == null) { return false; }
            var types = new[]
                           {
                              typeof (Enum),
                              typeof (String),
                              typeof (Char),
                              typeof (Guid),

                              typeof (Boolean),
                              typeof (Byte),
                              typeof (Int16),
                              typeof (Int32),
                              typeof (Int64),
                              typeof (Single),
                              typeof (Double),
                              typeof (Decimal),

                              typeof (SByte),
                              typeof (UInt16),
                              typeof (UInt32),
                              typeof (UInt64),

                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          }.ToList();

            if (types.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            return type.IsEnum;
        }

        public static bool IsPrimitiveOrPrimitiveNulable(this Type type)
        {

            var types = new[]
                           {
                              typeof (Enum),
                              typeof (String),
                              typeof (Char),
                              typeof (Guid),

                              typeof (Boolean),
                              typeof (Byte),
                              typeof (Int16),
                              typeof (Int32),
                              typeof (Int64),
                              typeof (Single),
                              typeof (Double),
                              typeof (Decimal),

                              typeof (SByte),
                              typeof (UInt16),
                              typeof (UInt32),
                              typeof (UInt64),

                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          }.ToList();

            var nullTypes = from t in types
                            where t.IsValueType
                            select typeof(Nullable<>).MakeGenericType(t);

            types.Concat(nullTypes);


            if (types.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            var nut = Nullable.GetUnderlyingType(type);
            return nut != null && nut.IsEnum;
        }

        public static bool IsList(this Type type)
        {
            return (type.GetNonNullableType().GetInterface(nameof(ICollection)) != null);
            //return type.Name.StartsWith("List`");
        }

        public static bool IsFluentEntity(this Type type)
        {
            return type.GetNonNullableType().Is(typeof(FluentEntity));
        }

        public static bool IsFluentEntity(this object obj)
        {
            return obj?.GetType().GetNonNullableType().Is(typeof(FluentEntity)) ?? false;
        }
        public static Type GetTaskType(this Type type)
        {
            if (type == typeof(Task)) { return null; }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        public static Type GetListTypeNonNull(this Type type)
        {
            if (type.IsList() && type.IsGenericType)
            {
                return type.GenericTypeArguments[0].GetNonNullableType();
            }

            return type?.GetElementType()?.GetNonNullableType() ?? type.GetNonNullableType();
        }

        public static object GetMaxValueOfNumber(this Type numberType)
        {
            numberType = numberType.GetNonNullableType();

            if (numberType.IsNumeric())
            {
                var value = numberType.GetField(nameof(int.MaxValue)).GetValue(null);
                return Convert.ChangeType(value, typeof(double));
            }
            else
            {
                throw new InvalidOperationException($"This operation is valid only for numbers. {nameof(GetMaxValueOfNumber)}");
            }
        }

        private static FluentJsonFormAttribute GetFluentJsonFormAttributeByType(this Type type)
        {
            var form = type.GetCustomAttribute<FluentJsonFormAttribute>(true);
            if (form == null)
            {
                form = new FluentJsonFormAttribute
                {
                    desc = type.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? type.Name,
                    group = "",
                    name = type.Name.ToFluentJsonStringNormalized(),
                    propName = type.Name.ToFluentJsonStringNormalized(),
                    Type = type
                };
            }

            return form;
        }

        private static FluentJsonPropertyAttribute GetFluentJsonPropertyAttributeByProperty(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<FluentJsonPropertyAttribute>(true);
            if (attr == null)
            {
                attr = new FluentJsonPropertyAttribute
                {
                    desc = property.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? property.Name,
                    group = "",
                    name = property.Name,
                    propName = property.Name.ToFluentJsonStringNormalized(),
                    Type = property.PropertyType,
                    Enums = null,
                    FkDestinal = null,
                    FluentAggregation = null,
                    FluentComposition = null,
                    form = EnumForm.TEXTBOX,
                    grid = property.Name,
                    IsEnum = property.PropertyType.IsNullableEnum(),
                    IsFk = false,
                    IsKey = false,
                    IsFluentUniqueKeyKey = false,
                    IsList = property.PropertyType.IsList(),
                    IsNullable = property.PropertyType.IsOfNullableType(),
                    min = property.GetCustomAttribute<MinLengthAttribute>(true)?.Length ?? 0,
                    max = property.GetCustomAttribute<MaxLengthAttribute>(true)?.Length ?? 0,
                };
            }

            attr.FluentAggregation = property.GetCustomAttribute<FluentManyToManyAggregationAttribute>(true) ?? property.GetCustomAttribute<FluentAggregationAttribute>(true);
            attr.FluentComposition = property.GetCustomAttribute<FluentCompositionAttribute>(true);
            attr.IsKey = property.GetCustomAttributeAny<KeyAttribute>();
            attr.IsFluentUniqueKeyKey = property.GetCustomAttributeAny<FluentUniqueKeyAttribute>();
            attr.IsList = property.PropertyType.IsList();
            attr.required = attr.required || property.GetCustomAttributeAny<RequiredAttribute>(true);
            attr.IsNullable = (property.PropertyType.IsOfNullableType() && !attr.required);
            attr.Type = property.PropertyType.GetNonNullableType();

            if (attr.max == 0 && property.PropertyType.IsNumeric())
            {
                attr.max = property.PropertyType.GetMaxValueOfNumber().FluentCast<double>();
            }

            if (property.GetCustomAttributeAny<JsonIgnoreAttribute>())
            {
                attr.form = EnumForm.NONE;
            }

            return attr;
        }

        public static FluentJsonSchema GetFluentJsonSchema(this Type type, bool tablet)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            type = type.GetListTypeNonNull();
            var form = GetFluentJsonFormAttributeByType(type);

            form.propName = type.Name.ToFluentJsonStringNormalized();
            form.Type = type.GetNonNullableType();

            var root = new FluentJsonSchema
            {
                FluentJsonForm = form,
                Properties = new List<FluentJsonPropertyAttribute>()
            };

            type.GetProperties().ToList().ForEach(property =>
            {
                var attr = GetFluentJsonPropertyAttributeByProperty(property);
                if (attr.form == EnumForm.NONE)
                {
                    return;
                }

                if (attr.FluentAggregation != null)
                {
                    if (attr.FluentAggregation?.GetType()?.Is(typeof(FluentManyToManyAggregationAttribute)) == true)
                    {

                    }

                    if (property.PropertyType.IsList())
                    {
                        // return; //Todo - Ignorando agregação em lista enquanto não é implementada
                    }

                    attr.FluentAggregation.SetType(property.PropertyType.GetListTypeNonNull().Name);
                    attr.FluentAggregation.SetName(property.Name);
                    attr.FluentAggregation.FluentFilter = property.GetCustomAttribute<FluentFilterAttribute>();
                    if (attr.FluentAggregation.FluentFilter != null)
                    {
                        attr.FluentAggregation.FluentFilter.PropertyName = property.Name.ToFluentJsonStringNormalized();
                        if (attr.FluentAggregation.FluentFilter.FieldsToClear != null)
                        {
                            attr.FluentAggregation.FluentFilter.FieldsToClear = attr.FluentAggregation.FluentFilter.FieldsToClear.Select(x => x.ToFluentJsonStringNormalized()).ToArray();
                        }
                    }
                }

                if (attr.FluentComposition != null)
                {
                    attr.FluentComposition.SetType(property.PropertyType.Name);
                    attr.FluentComposition.SetName(property.Name);
                    attr.FluentComposition.Form = GetFluentJsonSchema(property.PropertyType, tablet);
                }

                if (property.GetCustomAttributeAny<RequiredAttribute>() || property.GetCustomAttributeAny<FluentRequiredAttribute>())
                {
                    attr.required = true;
                }

                if (property.PropertyType.IsNullableEnum())
                {
                    attr.IsEnum = true;
                    attr.Enums = new List<KeyValuePair<string, string>>();

                    foreach (var field in property.PropertyType.GetEnumFields())
                    {
                        if (field.Name.Equals("value__", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                        var value = field.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? field.Name;
                        attr.Enums.Add(new KeyValuePair<string, string>(field.Name, value));
                    }
                }

                attr.PropNameCaseSensitive = property.Name;
                attr.propName = property.Name.ToFluentJsonStringNormalized();
                root.Properties.Add(attr);
            });

            root.Properties = root.Properties
                                .GroupBy(x => x.group)
                                .SelectMany(x => x)
                                .ToList();

            root.Properties.Where(x => x.form == EnumForm.HIDDEN).ToList().ForEach(property =>
            {
                property.lGrid = 0;
                property.Row = 0;
            });

            var properties = root.Properties.Where(x => x.form != EnumForm.HIDDEN).ToList();
            properties.ForEach(property =>
            {
                if (tablet)
                {
                    property.lGrid *= 2;
                }

                if (property.lGrid == 0 || property.lGrid > 12) { property.lGrid = 12; }
                property.Row = 0;
            });

            {
                var grid = 0;
                var row = 1;
                var lastGroup = properties.FirstOrDefault()?.group ?? "";
                properties.ForEach(x =>
                {
                    if (lastGroup != x.group) { grid = 0; row++; lastGroup = x.group; }

                    if (grid + x.lGrid > 12)
                    {
                        row++;
                        grid = x.lGrid;
                    }
                    else
                    {
                        grid += x.lGrid;
                    }

                    x.Row = row;
                });
            }

            {
                var props = new List<FluentJsonPropertyAttribute>();
                var row = 1;

                properties.ForEach(property =>
                {
                    if (row != property.Row)
                    {
                        AdjustColumns(props);
                        props.Clear();
                        row++;
                    }

                    props.Add(property);

                });

                if (props.Count > 0)
                {
                    AdjustColumns(props);
                }
            }

            MappForengKey(root);
            return root;
        }

        private static void AdjustColumns(List<FluentJsonPropertyAttribute> props)
        {
            var sum = props.Sum(y => y.lGrid);
            var count = props.Count();
            int i = 0;

            while (sum < 12)
            {
                props[i].lGrid++;
                sum = props.Sum(y => y.lGrid);
                if (i + 1 == count) { i = 0; } else { i++; }
            }
        }

        private static void MappForengKey(FluentJsonSchema schema)
        {
            schema.Properties.ForEach(property =>
            {
                if (property.FluentAggregation == null) { return; }
                property.FluentAggregation.LocalKeys.ToList().ForEach(key =>
                {
                    var fkProperty = schema.Properties.Single(x => x.PropNameCaseSensitive.Equals(key));
                    fkProperty.IsFk = true;
                    fkProperty.FkDestinal = property.Type.GetCustomAttribute<FluentJsonFormAttribute>();
                    if (fkProperty.FkDestinal != null) { fkProperty.FkDestinal.Type = property.Type; }
                });
            });
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
            type = type.GetNonNullableType();

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
            return parameters?.Select(x => x?.ParameterType?.GetFluentDefaultValue())?.ToArray();
        }

        /// <summary>
        /// Obtem o nome amigável de um tipo. Exemplo: FluentSelectSpecification FluentEntity
        /// </summary>
        /// <param name="type">O tipo a ser tratado.</param>
        /// <param name="useGenericT">Se deve indicar os tipos genéricos como T. Exemplo com true: FluentSelectSpecification T, T. Exemplo com false: FluentSelectSpecification FluentEntity, TO </param>
        /// <returns>O nome amigável do tipo.</returns>
        public static string GetFriendlyName(this Type type, bool useGenericT = true, bool fullName = false, string complement = "")
        {
            if (type == null) { return "null"; }
            var friendlyName = fullName ? type.Name : type.Name;
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
                var typeParamName = GetFriendlyName(typeParameters[i], useGenericT, fullName, complement);
                typeParamName = useGenericT ? "T" : typeParamName;
                friendlyName += (i == 0 ? typeParamName : ", " + typeParamName);
            }

            friendlyName += ">";

            if (!string.IsNullOrWhiteSpace(complement)) { friendlyName = string.Format(complement, friendlyName); }
            return friendlyName;
        }

        public static T FluentCast<T>(this object obj)
        {
            if (obj == null)
            {
                return default;
            }

            if (obj is T value)
            {
                return value;
            }
            else
            {
                throw new InvalidOperationException($"{obj.GetType().Name} is not a {typeof(T).Name}.");
            }
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