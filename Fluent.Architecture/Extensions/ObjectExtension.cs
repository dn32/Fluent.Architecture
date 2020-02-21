using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dn32.infra.extensoes
{
    /// <summary>
    /// Extensão de objetos.
    /// </summary>
    public static class ObjectExtension
    {
        public static bool TryGetValue<TKey, TValue>(this Dictionary<string, TValue> dictionary, string key, StringComparison comparisonType, out TValue value)
        {
            value = dictionary.SingleOrDefault(x => string.Equals(x.Key, key, comparisonType)).Value;
            return value != null;
        }

        public static T DnClone<T>(this object obj1)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (obj1 is null)
            {
                return default;
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, obj1);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Verifica se dois objetos são iguais comparando os valores e não a referência.
        /// </summary>
        /// <param Nome="obj1">
        /// Primeiro objeto a ser comparado.
        /// </param>
        /// <param Nome="obj2">
        /// Segundo objeto a ser comparado.
        /// </param>
        public static bool CompareObjects(this object obj1, object obj2)
        {
            return obj1.GetAllDataOfObject() == obj2.GetAllDataOfObject();
        }

        public static object GetDbValue(this object value, PropertyInfo property = null)
        {
            if (value == null)
            {
                if (property != null)
                {
                    return $"'{property.PropertyType.GetDnDefaultValue()?.ToString()?.Replace("'", "´")}'";
                }

                return null;
            }

            var type = value.GetType();

            if (type == typeof(string) || type == typeof(String))
            {
                return $"'{value?.ToString()?.Replace("'", "´")}'";
            }

            if (type == typeof(int))
            {
                return value;
            }

            if (type == typeof(Guid))
            {
                return $"'{value}'";
            }

            if (value.GetType().IsNullableEnum())
            {
                if (value.GetType().IsDefined(typeof(DnUsarStringParaEnumeradoresNoBdAtributo)))
                {
                    return (int)value;
                }

                return $"'{value.ToString()}'";
            }

            return value;
        }

        /// <summary>
        /// Verifica se um objeto é nulo ou vazio
        /// </summary>
        /// <param Nome="value">
        /// Objeto a ser verificado.
        /// </param>
        /// <returns>
        /// Se o objeto é nulo ou vazio.
        /// </returns>
        public static bool IsDnNull(this object value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();

            if (type == typeof(string) || type == typeof(String))
            {
                return string.IsNullOrWhiteSpace(value.ToString());
            }

            if (type == typeof(int))
            {
                return (int)value == 0;
            }

            if (type == typeof(Guid))
            {
                return (Guid)value == Guid.Empty;
            }

            if (value.GetType().IsNullableEnum())
            {
                return (int)value == 0;
            }

            return value == value.GetType().GetDnDefaultValue();
        }

        /// <summary>
        /// Obtem todos os dados de um objeto, incluindo de campos e propriedades privadas.
        /// </summary>
        /// <param Nome="objectToCheck">Objeto a ser avaliado.</param>
        /// <returns>
        /// Json com todos os dados do onjeto.
        /// </returns>
        public static string GetAllDataOfObject(this object objectToCheck)
        {
            var propertyData = new List<NameAndValue>();
            GetAllFieldsDataOfObject(objectToCheck, propertyData);
            return JsonConvert.SerializeObject(propertyData, Formatting.None);
        }

        /// <summary>
        /// Obtem todos o nome e valor de todos os campos de um objeto.
        /// </summary>
        /// <param Nome="obj">Objeto a ser avaliado.</param>
        /// <param Nome="propertyData">
        /// Lista de valores.
        /// </param>
        /// <returns>A lista com nome e valor de todos os campos do objeto.</returns>
        private static void GetAllFieldsDataOfObject(this object obj, List<NameAndValue> propertyData)
        {
            if (obj == null || obj is IQueryable)
            {
                return;
            }

            var objectType = obj.GetType();

            if (obj is ICollection collection)
            {
                foreach (var el in collection)
                {
                    GetAllFieldsDataOfObject(el, propertyData);
                }
            }
            else
            {
                var items = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                var type = objectType;

                while (type != null && type.Is(typeof(object)) && type != typeof(object))
                {
                    type = type?.BaseType ?? throw new InvalidOperationException("Parameter not set");
                    items.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
                }

                foreach (var item in items)
                {
                    if (item.FieldType.IsPrimitive || item.FieldType.IsValueType || item.FieldType == typeof(string))
                    {
                        propertyData.Add(new NameAndValue { Name = item?.Name?.Replace("k__BackingField", string.Empty, StringComparison.OrdinalIgnoreCase) ?? "", Value = item?.GetValue(obj) });
                    }
                    else if (item.FieldType.IsClass && !typeof(IEnumerable).IsAssignableFrom(item.FieldType))
                    {
                        GetAllFieldsDataOfObject(item.GetValue(obj), propertyData);
                    }
                    else
                    {
                        if (!(item.GetValue(obj) is IEnumerable enumerablePropObj1))
                        {
                            continue;
                        }

                        foreach (var propItem in enumerablePropObj1)
                        {
                            GetAllFieldsDataOfObject(propItem, propertyData);
                        }
                    }
                }
            }
        }
    }
}