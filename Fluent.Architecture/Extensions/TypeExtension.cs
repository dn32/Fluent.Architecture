using dn32.infra.atributos;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Util;
using dn32.infra.servicos;
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
using System.Threading.Tasks;
using dn32.infra.dados;
using dn32.infra.enumeradores;
using dn32.infra.extensoes;

namespace dn32.infra.extensoes
{
    /// <summary>
    /// Extensão de Tipo.
    /// </summary>
    public static class TypeExtension
    {
        private static readonly ConcurrentDictionary<Type, object> TypeDefaults = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Obtem o valor padrão de um tipo.
        /// Muito útil para preencher construtores de classes por reflexão.
        /// </summary>
        /// <param Nome="type">O tipo a ser avaliado.</param>
        /// <returns>O valor padrão do tipo.</returns>
        public static object GetDnDefaultValue(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            type = type.GetNonNullableType();
            return type.IsValueType ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance) : null;
        }

        public static bool DnEquals(this object value1, object value2)
        {
            if (value1 == null && value2 == null) { return true; }
            return value1?.ToString() == value2?.ToString();
        }

        //Todo2 doc
        public static TX GetDefaultValue<TX>()
        {
            return (TX)typeof(TX).GetDnDefaultValue();
        }

        public static bool IsKey(this MemberInfo info)
        {
            return info.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || info.IsDefined(typeof(KeyAttribute), true);
        }

        //public static bool GetCustomAttributeAny<T>(this Tipo type, bool inherit = false) where T : Attribute
        //{
        //    return type.GetCustomAttribute<T>(inherit) != null;
        //}

        //public static bool GetCustomAttributeAny<T>(this MemberInfo methodInfo, bool inherit = false) where T : Attribute
        //{
        //    return methodInfo.GetCustomAttribute<T>(inherit) != null;
        //}

        //public static bool GetCustomAttributeAny<T>(this ParameterInfo methodInfo, bool inherit = false) where T : Attribute
        //{
        //    return methodInfo.GetCustomAttribute<T>(inherit) != null;
        //}

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

            return type.GetDnDefaultValue();
        }

        public static string GetExampleValueString(this Type type)
        {
            var obj = type.GetExampleValue();
            if (type.IsPrimitiveOrPrimitiveNulable()) { return obj.ToString(); }
            return obj.SerializarParaDnJson(Formatting.Indented);
        }

        public static object GetExampleValue(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            object obj;

            if (type.IsPrimitiveOrPrimitiveNulable()) { return type.GetPrimitiveExampleValue(); }
            if (type.IsGenericType) { return null; }

            try
            {
                if (type.IsList())
                {
                    obj = Array.CreateInstance(type, 2);
                }
                else
                {
                    obj = Activator.CreateInstance(type);
                }
            }
            catch (MissingMethodException)
            {

                return type.GetDnDefaultValue();
            }

            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsNullableEnum())
                {
                    var firstEnum = Enum.GetValues(property.PropertyType.GetTypeByNullType()).GetValue(0);
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
            //return type.Name.StartsWith("Listar`");
        }

        public static bool IsDnEntity(this Type type)
        {
            return type.GetNonNullableType().Is(typeof(DnEntidade));
        }

        public static bool IsDnEntity(this object obj)
        {
            return obj?.GetType().GetNonNullableType().Is(typeof(DnEntidade)) ?? false;
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

        public static object GetMaxValueOfNumber(this Type numberType_)
        {
            if (numberType_ is null) { throw new ArgumentNullException(nameof(numberType_)); }
            var numberType = numberType_.GetNonNullableType();

            if (numberType?.IsNumeric() == true)
            {
                var value = numberType?.GetField(nameof(int.MaxValue))?.GetValue(null) ?? int.MaxValue;
                return Convert.ChangeType(value ?? int.MaxValue, typeof(double));
            }
            else
            {
                throw new InvalidOperationException($"This operation is valid only for numbers. {nameof(GetMaxValueOfNumber)}");
            }
        }

        private static DnFormularioJsonAtributo GetDnJsonFormAttributeByType(this Type type)
        {
            var form = type.GetCustomAttribute<DnFormularioJsonAtributo>(true);
            if (form == null)
            {
                form = new DnFormularioJsonAtributo
                {
                    Descricao = type.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? type.Name,
                    Grupo = "",
                    Nome = type.Name.ToDnJsonStringNormalized(),
                    NomeDaPropriedade = type.Name.ToDnJsonStringNormalized(),
                    Tipo = type
                };
            }

            return form;
        }

        private static DnPropriedadeJsonAtributo GetDnJsonPropertyAttributeByProperty(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<DnPropriedadeJsonAtributo>(true);
            if (attr == null)
            {
                attr = new DnPropriedadeJsonAtributo
                {
                    Descricao = property.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? property.Name,
                    Grupo = "",
                    Nome = property.Name,
                    NomeDaPropriedade = property.Name.ToDnJsonStringNormalized(),
                    Tipo = property.PropertyType,
                    Propriedade = property,
                    Enumeradores = null,
                    DestinoDeChaveExterna = null,
                    Agregacao = null,
                    Composicao = null,
                    Formulario = EnumTipoDeComponenteDeFormularioDeTela.Texto,
                    Grid = property.Name,
                    EhEnumerador = property.PropertyType.IsNullableEnum(),
                    EhChaveExterna = false,
                    EhChave = false,
                    EhDnChaveUnica = false,
                    EhLista = property.PropertyType.IsList(),
                    PermiteNulo = property.PropertyType.IsOfNullableType(),
                    Minimo = property.GetCustomAttribute<MinLengthAttribute>(true)?.Length ?? 0,
                    Maximo = property.GetCustomAttribute<MaxLengthAttribute>(true)?.Length ?? 0
                };
            }

            attr.OperacaoDeCondicional = property.GetCustomAttributes<DnOperacaoDeCondicionalDeTelaAtributo>();
            attr.Agregacao = property.GetCustomAttribute<DnAgregacaoDeMuitosParaMuitosAtributo>(true) ?? property.GetCustomAttribute<DnAgregacaoAtributo>(true);
            attr.Composicao = property.GetCustomAttribute<DnComposicaoAtributo>(true);
            attr.EhChave = property.IsDefined(typeof(KeyAttribute));
            attr.EhDnChaveUnica = property.IsDefined(typeof(DnChaveUnicaAtributo));
            attr.EhLista = property.PropertyType.IsList();
            attr.EhRequerido = attr.EhRequerido || property.IsDefined(typeof(RequiredAttribute), true);
            attr.PermiteNulo = (property.PropertyType.IsOfNullableType() && !attr.EhRequerido);
            attr.Tipo = property.PropertyType.GetNonNullableType();
            attr.Propriedade = property;

            if (attr.Maximo == 0 && property.PropertyType.IsNumeric())
            {
                attr.Maximo = property.PropertyType.GetMaxValueOfNumber().DnCast<double>();
            }

            if (property.IsDefined(typeof(JsonIgnoreAttribute)))
            {
                attr.Formulario = EnumTipoDeComponenteDeFormularioDeTela.Nenhum;
            }

            return attr;
        }

        public static DnJsonSchema GetDnJsonSchema(this Type type, bool tablet)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            type = type.GetListTypeNonNull();
            var form = GetDnJsonFormAttributeByType(type);

            form.NomeDaPropriedade = type.Name.ToDnJsonStringNormalized();
            form.Tipo = type.GetNonNullableType();

            var root = new DnJsonSchema
            {
                Formulario = form,
                Propriedades = new List<DnPropriedadeJsonAtributo>()
            };

            type.GetProperties().ToList().ForEach(property =>
            {
                if (property == null) { return; }

                var attr = GetDnJsonPropertyAttributeByProperty(property);
                attr.Propriedade = property;
                if (attr.Formulario == EnumTipoDeComponenteDeFormularioDeTela.Nenhum)
                {
                    return;
                }

                if (attr.Agregacao != null)
                {
                    if (attr.Agregacao.GetType()?.Is(typeof(DnAgregacaoDeMuitosParaMuitosAtributo)) == true)
                    {

                    }

                    if (property.PropertyType.IsList())
                    {
                        // return; //Todo - Ignorando agregação em lista enquanto não é implementada
                    }

                    attr.Agregacao.DefinirTipo(property.PropertyType.GetListTypeNonNull().Name);
                    attr.Agregacao.DefinirNome(property.Name);
                    attr.Agregacao.Filtro = property.GetCustomAttribute<DnFiltroAtributo>();
                    if (attr.Agregacao.Filtro != null)
                    {
                        attr.Agregacao.Filtro.NomeDaPropriedade = property.Name.ToDnJsonStringNormalized();
                        if (attr.Agregacao.Filtro.CamposParaLimpar != null)
                        {
                            attr.Agregacao.Filtro.CamposParaLimpar = attr.Agregacao.Filtro.CamposParaLimpar.Select(x => x.ToDnJsonStringNormalized()).ToArray();
                        }
                    }
                }

                if (attr.Composicao != null)
                {
                    attr.Composicao.DefinirTipo(property.PropertyType.GetListTypeNonNull().Name);
                    attr.Composicao.DefinirNome(property.Name);
                    attr.Composicao.Formulario = GetDnJsonSchema(property.PropertyType, tablet);
                }

                if (property.IsDefined(typeof(RequiredAttribute)) || property.IsDefined(typeof(DnRequeridoAtributo)))
                {
                    attr.EhRequerido = true;
                }

                if (property.PropertyType.IsNullableEnum())
                {
                    attr.EhEnumerador = true;
                    attr.Enumeradores = new List<KeyValuePair<string, string>>();

                    foreach (var field in property.PropertyType.GetEnumFields())
                    {
                        if (field.Name.Equals("value__", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                        var value = field.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? field.Name;
                        attr.Enumeradores.Add(new KeyValuePair<string, string>(field.Name, value));
                    }
                }

                attr.NomeDaPropriedadeCaseSensitive = property.Name;
                attr.NomeDaPropriedade = property.Name.ToDnJsonStringNormalized();
                root.Propriedades.Add(attr);
            });

            root.Propriedades = root.Propriedades
                                .GroupBy(x => x.Grupo)
                                .SelectMany(x => x)
                                .ToList();

            root.Propriedades.Where(x => x.Formulario == EnumTipoDeComponenteDeFormularioDeTela.Hidden).ToList().ForEach(property =>
            {
                property.LayoutDeGrid = 0;
                property.Linha = 0;
            });

            var properties = root.Propriedades.Where(x => x.Formulario != EnumTipoDeComponenteDeFormularioDeTela.Hidden).ToList();
            properties.ForEach(property =>
            {
                if (tablet)
                {
                    property.LayoutDeGrid *= 2;
                }

                if (property.LayoutDeGrid == 0 || property.LayoutDeGrid > 12) { property.LayoutDeGrid = 12; }
                property.Linha = 0;
            });

            {
                var grid = 0;
                var row = 1;
                var lastGroup = properties.FirstOrDefault()?.Grupo ?? "";
                properties.ForEach(x =>
                {
                    if (lastGroup != x.Grupo) { grid = 0; row++; lastGroup = x.Grupo; }

                    if (grid + x.LayoutDeGrid > 12)
                    {
                        row++;
                        grid = x.LayoutDeGrid;
                    }
                    else
                    {
                        grid += x.LayoutDeGrid;
                    }

                    x.Linha = row;
                });
            }

            {
                var props = new List<DnPropriedadeJsonAtributo>();
                var row = 1;

                properties.ForEach(property =>
                {
                    if (row != property.Linha)
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

        private static void AdjustColumns(List<DnPropriedadeJsonAtributo> props)
        {
            var sum = props.Sum(y => y.LayoutDeGrid);
            var count = props.Count();
            int i = 0;

            while (sum < 12)
            {
                props[i].LayoutDeGrid++;
                sum = props.Sum(y => y.LayoutDeGrid);
                if (i + 1 == count) { i = 0; } else { i++; }
            }
        }

        private static void MappForengKey(DnJsonSchema schema)
        {
            schema.Propriedades.ForEach(property =>
            {
                if (property.Agregacao == null) { return; }
                property.Agregacao.ChavesLocais.ToList().ForEach(key =>
                {
                    var fkProperty = schema.Propriedades.Single(x => x.NomeDaPropriedadeCaseSensitive.Equals(key));
                    fkProperty.EhChaveExterna = true;
                    fkProperty.DestinoDeChaveExterna = property.Tipo.GetCustomAttribute<DnFormularioJsonAtributo>();
                    if (fkProperty.DestinoDeChaveExterna != null) { fkProperty.DestinoDeChaveExterna.Tipo = property.Tipo; }
                });
            });
        }

#nullable disable
#pragma warning disable CS8632 // The annotation for nullable Referencia types should only be used in code within a '#nullable' annotations context.
        public static bool IsNullableEnum(this Type? t)
#pragma warning restore CS8632 // The annotation for nullable Referencia types should only be used in code within a '#nullable' annotations context.
        {
            if (t?.IsEnum == true) { return true; }
            var u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
#nullable restore

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
            return parameters?.Select(x => x?.ParameterType?.GetDnDefaultValue())?.ToArray();
        }

        /// <summary>
        /// Obtem o nome amigável de um tipo. Exemplo: DnSelectSpecification DnEntity
        /// </summary>
        /// <param Nome="type">O tipo a ser tratado.</param>
        /// <param Nome="useGenericT">Se deve indicar os tipos genéricos como T. Exemplo com true: DnSelectSpecification T, T. Exemplo com false: DnSelectSpecification DnEntity, TO </param>
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

        public static Type GetSpecializedService(this Type serviceType)
        {
            if (serviceType is null) { throw new ArgumentNullException(nameof(serviceType)); }
            if (serviceType.BaseType is null) { throw new ArgumentNullException(nameof(serviceType.BaseType)); }

            if (serviceType.Name == "DnDynamicProxy")
            {
                serviceType = serviceType.BaseType;
            }

            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Servicos.TryGetValue(entityType, out serviceType))
                {
                    var type = (Setup.ConfiguracoesGlobais.GenericServiceType) ?? typeof(DnServico<>);
                    serviceType = type.MakeGenericType(entityType);
                }
            }

            return serviceType;
        }
    }
}