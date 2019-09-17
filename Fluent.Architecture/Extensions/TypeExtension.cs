// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Entities;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

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
            return TypeDefaults.GetOrAdd(type, Activator.CreateInstance);
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


        //Todo2 doc
        public static TX GetDefaultValue<TX>()
        {
            return (TX)typeof(TX).GetDefaultValue();
        }

        //Todo2 doc
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

        //Todo2 doc
        public static bool Is(this Type t1, Type t2)
        {
            return t1.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t2) ||
                   t2.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t1) ||
                   t1 == t2 || t1.IsSubclassOf(t2) || t2.IsAssignableFrom(t1) || t2.IsSubclassOf(t1) || t1.IsAssignableFrom(t2);
        }

        public static object GetExampleValue(this Type type)
        {
            var obj = Activator.CreateInstance(type);

            //if (type.Name == "List`1")
            //{//Todo - implementar para lista
            //    return Activator;
            //}

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
                        var newValue = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
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

        public static FieldInfo[] GetEnumFields(this Type type)
        {
            var nullValue = Nullable.GetUnderlyingType(type);
            type = nullValue ?? type;
            return type.GetFields();
        }

        public static bool IsPrimitive(this Type type)
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

        public static FluentJsonSchema GetFluentJsonSchema(this Type type, bool tablet)
        {
            if (type.Name == "List`1")
            {
                type = type.GenericTypeArguments[0];
            }

            var form = type.GetCustomAttribute<FluentJsonFormAttribute>();
            form.propName = type.Name;

            var root = new FluentJsonSchema
            {
                FluentJsonForm = form,
                Properties = new List<FluentJsonPropertyAttribute>()
            };

            type.GetProperties().ToList().ForEach(x =>
            {
                var attr = x.GetCustomAttribute<FluentJsonPropertyAttribute>();
                if (attr == null || attr.form == EnumForm.NONE)
                {
                    return;
                }

                attr.FluentAggregation = x.GetCustomAttribute<FluentAggregationAttribute>(true);
                attr.FluentComposition = x.GetCustomAttribute<FluentCompositionAttribute>(true);
                attr.IsKey = x.GetCustomAttribute<KeyAttribute>() != null;
                attr.IsList = x.PropertyType.Name == "List`1";
                attr.IsNullable = x.PropertyType.Equals(typeof(string)) || x.PropertyType.IsOfNullableType();

                if (attr.FluentAggregation != null)
                {
                    if (x.PropertyType.Name == "List`1")
                    {
                        return; //Ignorando agregação em lista enquanto não é implementada
                    }

                    attr.FluentAggregation.SetType(x.PropertyType.Name);
                    attr.FluentAggregation.SetName(x.Name);
                }

                if (attr.FluentComposition != null)
                {
                    attr.FluentComposition.SetType(x.PropertyType.Name);
                    attr.FluentComposition.SetName(x.Name);
                    attr.FluentComposition.Form = GetFluentJsonSchema(x.PropertyType, tablet);
                }

                if (x.PropertyType.IsNullableEnum())
                {
                    attr.IsEnum = true;
                    attr.Enums = new List<KeyValuePair<string, string>>();

                    foreach (var field in x.PropertyType.GetEnumFields())
                    {
                        if (field.Name.Equals("value__", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                        var value = field.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? field.Name;
                        attr.Enums.Add(new KeyValuePair<string, string>(field.Name, value));
                    }
                }

                attr.propName = x.Name;
                root.Properties.Add(attr);
            });

            root.Properties = root.Properties
                                .GroupBy(x => x.group)
                                .SelectMany(x => x)
                                .ToList();

            root.Properties.Where(x => x.form == EnumForm.HIDDEN).ToList().ForEach(property =>
            {
                property.LGrid = 0;
                property.Row = 0;
            });

            var properties = root.Properties.Where(x => x.form != EnumForm.HIDDEN).ToList();
            properties.ForEach(property =>
            {
                if (tablet)
                {
                    property.LGrid *= 2;
                }

                if (property.LGrid == 0 || property.LGrid > 12) { property.LGrid = 12; }
                property.Row = 0;
            });

            {
                var grid = 0;
                var row = 1;
                properties.ForEach(x =>
                {
                    if (grid + x.LGrid > 12)
                    {
                        row++;
                        grid = x.LGrid;
                    }
                    else
                    {
                        grid += x.LGrid;
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
                        CustomJsonResult.AdjustColumns(props, row);
                        props.Clear();
                        row++;
                    }

                    props.Add(property);

                });

                if (props.Count > 0)
                {
                    CustomJsonResult.AdjustColumns(props, row);
                }
            }

            return root;
        }

        public static bool IsNullableEnum(this Type t)
        {
            if (t.IsEnum == true) { return true; }
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

        public static T FluentCast<T>(this object obj)
        {
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
            if (serviceType.Name == "FluentDynamicProxy")
            {
                serviceType = serviceType.BaseType ?? serviceType; ;
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